import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import authService from '../services/authService'
import API_BASE_URL from '../services/api'

function DepartmentsPage() {
  const navigate = useNavigate()

  const [departments, setDepartments] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const loadDepartments = async () => {
      try {
        const response = await fetch(
          `${API_BASE_URL}/api/Departments`,
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
            setError('You do not have permission to view departments.')
            return
          }

          setError('Unable to load departments.')
          return
        }

        const data = await response.json()

        setDepartments(data)
      } catch {
        setError('Unable to connect to the server.')
      } finally {
        setLoading(false)
      }
    }

    loadDepartments()
  }, [])

  const handleDelete = async (departmentId) => {
    const confirmed = window.confirm(
      'Are you sure you want to delete this department?',
    )

    if (!confirmed) {
      return
    }

    try {
      const response = await fetch(
        `${API_BASE_URL}/api/Departments/${departmentId}`,
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
          setError('You do not have permission to delete departments.')
          return
        }

        if (response.status === 404) {
          setError('Department not found.')
          return
        }

        setError('Unable to delete department.')
        return
      }

      setDepartments((previous) =>
        previous.filter(
          (department) => department.id !== departmentId,
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
          <h2 className="mb-1">Departments</h2>
          <p className="text-muted mb-0">
            Manage departments.
          </p>
        </div>

        <button
          type="button"
          className="btn btn-primary"
          onClick={() => navigate('/departments/create')}
        >
          Add Department
        </button>
      </div>

      {loading && (
        <div className="d-flex align-items-center gap-2">
          <div
            className="spinner-border spinner-border-sm"
            role="status"
            aria-hidden="true"
          ></div>
          <span>Loading departments...</span>
        </div>
      )}

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      {!loading && !error && (
        <div className="table-responsive">
          <table className="table table-striped table-hover">
            <thead>
              <tr>
                <th>Name</th>
                <th>Action</th>
              </tr>
            </thead>

            <tbody>
              {departments.map((department) => (
                <tr key={department.id}>
                  <td>{department.name}</td>

                  <td>
                    <div className="d-flex gap-2">
                      <button
                        type="button"
                        className="btn btn-sm btn-warning"
                        onClick={() =>
                          navigate(`/departments/${department.id}/edit`)
                        }
                      >
                        Edit
                      </button>

                      <button
                        type="button"
                        className="btn btn-sm btn-danger"
                        onClick={() =>
                          handleDelete(department.id)
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
      )}
    </div>
  )
}

export default DepartmentsPage