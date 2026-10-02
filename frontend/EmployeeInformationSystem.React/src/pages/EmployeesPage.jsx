import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import authService from '../services/authService'

function EmployeesPage() {
  const navigate = useNavigate()
  const [employees, setEmployees] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  const [search, setSearch] = useState('')
  const [pageNumber, setPageNumber] = useState(1)
  const [pageSize] = useState(10)
  const [totalCount, setTotalCount] = useState(0)
  const [totalPages, setTotalPages] = useState(0)

  useEffect(() => {
    const loadEmployees = async () => {
      try {
        const response = await fetch(
          `http://localhost:8080/api/Employees?pageNumber=${pageNumber}&pageSize=${pageSize}&search=${encodeURIComponent(search)}`,
          {
            headers: {
              Authorization: `Bearer ${authService.getToken()}`,
            },
          },
        )

        if (!response.ok) {
          if (response.status === 401) {
            setError('Your session has expired. Please login again.')
            return
          }

          if (response.status === 403) {
            setError('You do not have permission to view employees.')
            return
          }

          setError('Unable to load employees.')
          return
        }

        const data = await response.json()

        setEmployees(data.items)
        setTotalCount(data.totalCount)
        setTotalPages(data.totalPages)

      } catch {
        setError('Unable to connect to the server.')
      } finally {
        setLoading(false)
      }
    }

    loadEmployees()
}, [pageNumber, pageSize, search])

const handleDelete = async (employeeId) => {
  const confirmed = window.confirm(
    'Are you sure you want to delete this employee?',
  )

  if (!confirmed) {
    return
  }

  try {
    const response = await fetch(
      `http://localhost:8080/api/Employees/${employeeId}`,
      {
        method: 'DELETE',
        headers: {
          Authorization: `Bearer ${authService.getToken()}`,
        },
      },
    )

    if (!response.ok) {
      if (response.status === 401) {
        setError('Your session has expired. Please login again.')
        return
      }

      if (response.status === 403) {
        setError('You do not have permission to delete employees.')
        return
      }

      if (response.status === 404) {
        setError('Employee not found.')
        return
      }

      setError('Unable to delete employee.')
      return
    }

    setEmployees((previous) =>
      previous.filter(
        (employee) => employee.employeeId !== employeeId,
      ),
    )
  } catch {
    setError('Unable to connect to the server.')
  }
}


  return (
    <div className="container py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2 className="mb-1">Employees</h2>
          <p className="text-muted mb-0">
            Manage employee information.
          </p>
        </div>

        <button
          type="button"
          className="btn btn-primary"
          onClick={() => navigate('/employees/create')}
        >
          Add Employee
        </button>
      </div>

      <div className="row mb-3">
        <div className="col-md-6">
          <input
            type="text"
            className="form-control"
            placeholder="Search employees by employee no, name, or email..."
            value={search}
            onChange={(event) => {
              setSearch(event.target.value)
              setPageNumber(1)
            }}
          />
        </div>
      </div>

      {loading && (
        <div className="d-flex align-items-center gap-2">
          <div
            className="spinner-border spinner-border-sm"
            role="status"
            aria-hidden="true"
          ></div>
          <span>Loading employees...</span>
        </div>
      )}

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      {!loading && !error && (
        <>
          <div className="table-responsive">
            <table className="table table-striped table-hover">
              <thead>
                <tr>
                  <th>Employee No</th>
                  <th>Name</th>
                  <th>Gender</th>
                  <th>Email</th>
                  <th>Mobile</th>
                  <th>Hire Date</th>
                  <th>Action</th>
                </tr>
              </thead>

              <tbody>
                {employees.map((employee) => (
                  <tr key={employee.employeeId}>
                    <td>{employee.employeeNo}</td>

                    <td>
                      {employee.firstName}{' '}
                      {employee.middleName
                        ? `${employee.middleName} `
                        : ''}
                      {employee.lastName}
                    </td>

                    <td>{employee.genderCode}</td>
                    <td>{employee.email ?? '-'}</td>
                    <td>{employee.mobileNo ?? '-'}</td>
                    <td>
                      {new Date(employee.hireDate).toLocaleDateString()}
                    </td>

                    <td>
                      <div className="d-flex gap-2">
                        <button
                          type="button"
                          className="btn btn-sm btn-primary"
                          onClick={() =>
                            navigate(`/employees/${employee.employeeId}`)
                          }
                        >
                          View
                        </button>

                        <button
                          type="button"
                          className="btn btn-sm btn-warning"
                          onClick={() =>
                            navigate(`/employees/${employee.employeeId}/edit`)
                          }
                        >
                          Edit
                        </button>

                        <button
                          type="button"
                          className="btn btn-sm btn-danger"
                          onClick={() =>
                            handleDelete(employee.employeeId)
                          }
                        >
                          Delete
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <div className="d-flex justify-content-between align-items-center mt-3">
            <div className="text-muted">
              Total: {totalCount} employees
            </div>

            <div className="d-flex gap-2">
              <button
                type="button"
                className="btn btn-outline-secondary"
                disabled={pageNumber === 1}
                onClick={() =>
                  setPageNumber((previous) => previous - 1)
                }
              >
                Previous
              </button>

              <span className="align-self-center">
                {totalPages === 0
                  ? 'No pages'
                  : `Page ${pageNumber} of ${totalPages}`}
              </span>

              <button
                type="button"
                className="btn btn-outline-secondary"
                disabled={pageNumber === totalPages}
                onClick={() =>
                  setPageNumber((previous) => previous + 1)
                }
              >
                Next
              </button>
            </div>
          </div>
        </>
      )}
    </div>
  )
}

export default EmployeesPage