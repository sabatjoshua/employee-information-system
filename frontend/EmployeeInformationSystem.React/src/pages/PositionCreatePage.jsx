import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import authService from '../services/authService'
import API_BASE_URL from '../services/api'

function PositionCreatePage() {
  const navigate = useNavigate()

  const [name, setName] = useState('')
  const [departmentId, setDepartmentId] = useState('')
  const [departments, setDepartments] = useState([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
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

  const handleSubmit = async (event) => {
    event.preventDefault()

    setError('')
    setSaving(true)

    try {
      const response = await fetch(
        `${API_BASE_URL}/api/Positions`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${authService.getToken()}`,
          },
          body: JSON.stringify({
            name,
            departmentId,
          }),
        },
      )

      if (!response.ok) {
        if (response.status === 401) {
          setError('Your session has expired. Please login again.')
          return
        }

        if (response.status === 403) {
          setError('You do not have permission to create positions.')
          return
        }

        setError('Unable to create position.')
        return
      }

      navigate('/positions')
    } catch {
      setError('Unable to connect to the server.')
    } finally {
      setSaving(false)
    }
  }

  if (loading) {
    return (
      <div className="container py-4">
        <div className="d-flex align-items-center gap-2">
          <div
            className="spinner-border spinner-border-sm"
            role="status"
            aria-hidden="true"
          ></div>
          <span>Loading departments...</span>
        </div>
      </div>
    )
  }

  return (
    <div className="container py-4">
      <h2 className="mb-1">Create Position</h2>

      <p className="text-muted mb-4">
        Add a new position.
      </p>

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit}>
        <div className="row mb-3">
          <label
            htmlFor="name"
            className="col-sm-2 col-form-label"
          >
            Name
          </label>

          <div className="col-sm-6">
            <input
              id="name"
              type="text"
              className="form-control"
              value={name}
              onChange={(event) => setName(event.target.value)}
              required
            />
          </div>
        </div>

        <div className="row mb-3">
          <label
            htmlFor="department"
            className="col-sm-2 col-form-label"
          >
            Department
          </label>

          <div className="col-sm-6">
            <select
              id="department"
              className="form-select"
              value={departmentId}
              onChange={(event) => setDepartmentId(event.target.value)}
              required
            >
              <option value="">Select Department</option>

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

        <div className="mt-4 text-center">
          <button
            type="submit"
            className="btn btn-primary"
            disabled={saving}
          >
            {saving ? 'Creating...' : 'Create Position'}
          </button>

          <button
            type="button"
            className="btn btn-secondary ms-2"
            onClick={() => navigate('/positions')}
            disabled={saving}
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  )
}

export default PositionCreatePage