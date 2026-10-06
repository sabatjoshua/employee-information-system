import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import authService from '../services/authService'
import API_BASE_URL from '../services/api'

function PositionsPage() {
  const navigate = useNavigate()

  const [positions, setPositions] = useState([])
  const [departments, setDepartments] = useState([])
  const [departmentId, setDepartmentId] = useState('')
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
      }
    }

    loadDepartments()
  }, [])

  useEffect(() => {
    const loadPositions = async () => {
      setLoading(true)
      setError('')

      try {
        if (!departmentId) {
          setPositions([])
          setLoading(false)
          return
        }

        const url =
          `${API_BASE_URL}/api/Positions?departmentId=${departmentId}`

        const response = await fetch(url, {
          headers: {
            Authorization: `Bearer ${authService.getToken()}`,
          },
        })

        if (!response.ok) {
          if (response.status === 401) {
            setError('Your session has expired. Please login again.')
            return
          }

          if (response.status === 403) {
            setError('You do not have permission to view positions.')
            return
          }

          setError('Unable to load positions.')
          return
        }

        const data = await response.json()
        setPositions(data)
      } catch {
        setError('Unable to connect to the server.')
      } finally {
        setLoading(false)
      }
    }

    loadPositions()
  }, [departmentId])

  const handleDelete = async (positionId) => {
    const confirmed = window.confirm(
      'Are you sure you want to delete this position?',
    )

    if (!confirmed) {
      return
    }

    try {
      const response = await fetch(
        `${API_BASE_URL}/api/Positions/${positionId}`,
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
          setError('You do not have permission to delete positions.')
          return
        }

        if (response.status === 404) {
          setError('Position not found.')
          return
        }

        setError('Unable to delete position.')
        return
      }

      setPositions((previous) =>
        previous.filter(
          (position) => position.id !== positionId,
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
          <h2 className="mb-1">Positions</h2>
          <p className="text-muted mb-0">
            Manage positions.
          </p>
        </div>

        <button
          type="button"
          className="btn btn-primary"
          onClick={() => navigate('/positions/create')}
        >
          Add Position
        </button>
      </div>

      <div className="row mb-3">
        <div className="col-md-6">
          <label
            htmlFor="department"
            className="form-label"
          >
            Department
          </label>

          <select
            id="department"
            className="form-select"
            value={departmentId}
            onChange={(event) => setDepartmentId(event.target.value)}
          >
            <option value="">All Departments</option>

            {departments.map((department) => (
              <option
                key={department.id}
                value={department.id}
              >
                {department.name}
              </option>
            ))}
          </select>
        </div>
      </div>

      {loading && (
        <div className="d-flex align-items-center gap-2">
          <div
            className="spinner-border spinner-border-sm"
            role="status"
            aria-hidden="true"
          ></div>

          <span>Loading positions...</span>
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
              {positions.map((position) => (
                <tr key={position.id}>
                  <td>{position.name}</td>

                  <td>
                    <div className="d-flex gap-2">
                      <button
                        type="button"
                        className="btn btn-sm btn-warning"
                        onClick={() =>
                          navigate(`/positions/${position.id}/edit`)
                        }
                      >
                        Edit
                      </button>

                      <button
                        type="button"
                        className="btn btn-sm btn-danger"
                        onClick={() =>
                          handleDelete(position.id)
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

export default PositionsPage