import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import authService from '../services/authService'

function EmployeeDetailsPage() {
  const { id } = useParams()
  const navigate = useNavigate()

  const [employee, setEmployee] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const loadEmployee = async () => {
      try {
        const response = await fetch(
          `http://localhost:8080/api/Employees/${id}`,
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
            setError('You do not have permission to view this employee.')
            return
          }

          if (response.status === 404) {
            setError('Employee not found.')
            return
          }

          setError('Unable to load employee.')
          return
        }

        const data = await response.json()
        setEmployee(data)
      } catch {
        setError('Unable to connect to the server.')
      } finally {
        setLoading(false)
      }
    }

    loadEmployee()
  }, [id])

  if (loading) {
    return (
      <div className="container py-4">
        <p>Loading employee...</p>
      </div>
    )
  }

const handleDelete = async () => {
  const confirmed = window.confirm(
    'Are you sure you want to delete this employee?',
  )

  if (!confirmed) {
    return
  }

  try {
    const response = await fetch(
      `http://localhost:8080/api/Employees/${id}`,
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

    navigate('/employees')
  } catch {
    setError('Unable to connect to the server.')
  }
}

  return (
    <div className="container py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>Employee Details</h2>

        <div className="d-flex gap-2 mt-4">
          <button
            type="button"
            className="btn btn-warning"
            onClick={() => navigate(`/employees/${id}/edit`)}
          >
            Edit
          </button>
          <button
            type="button"
            className="btn btn-danger"
            onClick={() => handleDelete()}
          >
            Delete
          </button>
          <button
            type="button"
            className="btn btn-secondary"
            onClick={() => navigate('/employees')}
          >
            Back to Employees
          </button>
        </div>
      </div>

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      {employee && !error && (
        <div className="card shadow-sm">
          <div className="card-body">
            <h4 className="card-title mb-4">
              {employee.firstName}{' '}
              {employee.middleName ? `${employee.middleName} ` : ''}
              {employee.lastName}
            </h4>

            <div className="row">
              <div className="col-md-6 mb-3">
                <strong>Employee No</strong>
                <div>{employee.employeeNo}</div>
              </div>

              <div className="col-md-6 mb-3">
                <strong>Gender</strong>
                <div>{employee.genderCode}</div>
              </div>

              <div className="col-md-6 mb-3">
                <strong>Email</strong>
                <div>{employee.email ?? '-'}</div>
              </div>

              <div className="col-md-6 mb-3">
                <strong>Mobile</strong>
                <div>{employee.mobileNo ?? '-'}</div>
              </div>

              <div className="col-md-6 mb-3">
                <strong>Birth Date</strong>
                <div>
                  {new Date(employee.birthDate).toLocaleDateString()}
                </div>
              </div>

              <div className="col-md-6 mb-3">
                <strong>Hire Date</strong>
                <div>
                  {new Date(employee.hireDate).toLocaleDateString()}
                </div>
              </div>

              <div className="col-md-6 mb-3">
                <strong>Department</strong>
                <div>{employee.departmentName}</div>
              </div>

              <div className="col-md-6 mb-3">
                <strong>Position</strong>
                <div>{employee.positionName}</div>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  )
}

export default EmployeeDetailsPage