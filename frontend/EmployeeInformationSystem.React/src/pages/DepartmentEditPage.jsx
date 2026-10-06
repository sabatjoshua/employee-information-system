import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import authService from '../services/authService'
import API_BASE_URL from '../services/api'

function DepartmentEditPage() {
  const navigate = useNavigate()
  const { id } = useParams()

  const [name, setName] = useState('')
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState('')

  useEffect(() => {
    const loadDepartment = async () => {
      try {
        const response = await fetch(
          `${API_BASE_URL}/api/Departments/${id}`,
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

          if (response.status === 404) {
            setError('Department not found.')
            return
          }

          setError('Unable to load department.')
          return
        }

        const data = await response.json()

        setName(data.name)
      } catch {
        setError('Unable to connect to the server.')
      } finally {
        setLoading(false)
      }
    }

    loadDepartment()
  }, [id])

  const handleSubmit = async (event) => {
    event.preventDefault()

    setError('')
    setSaving(true)

    try {
      const response = await fetch(
        `${API_BASE_URL}/api/Departments/${id}`,
        {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${authService.getToken()}`,
          },
          body: JSON.stringify({
            name,
          }),
        },
      )

      if (!response.ok) {
        if (response.status === 401) {
          setError('Your session has expired. Please login again.')
          return
        }

        if (response.status === 403) {
          setError('You do not have permission to update departments.')
          return
        }

        if (response.status === 404) {
          setError('Department not found.')
          return
        }

        setError('Unable to update department.')
        return
      }

      navigate('/departments')
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
          <span>Loading department...</span>
        </div>
      </div>
    )
  }

  return (
    <div className="container py-4">
      <h2 className="mb-1">Edit Department</h2>
      <p className="text-muted mb-4">
        Update department information.
      </p>

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      {!error && (
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

          <div className="mt-4 text-center">
            <button
              type="submit"
              className="btn btn-primary"
              disabled={saving}
            >
              {saving ? 'Saving...' : 'Save Changes'}
            </button>

            <button
              type="button"
              className="btn btn-secondary ms-2"
              onClick={() => navigate('/departments')}
              disabled={saving}
            >
              Cancel
            </button>
          </div>
        </form>
      )}
    </div>
  )
}

export default DepartmentEditPage