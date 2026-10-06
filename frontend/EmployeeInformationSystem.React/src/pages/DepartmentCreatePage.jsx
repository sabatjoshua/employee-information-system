import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import authService from '../services/authService'
import API_BASE_URL from '../services/api'

function DepartmentCreatePage() {
  const navigate = useNavigate()

  const [name, setName] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  const handleSubmit = async (event) => {
    event.preventDefault()

    setError('')
    setLoading(true)

    try {
      const response = await fetch(
        `${API_BASE_URL}/api/Departments`,
        {
          method: 'POST',
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
          setError('You do not have permission to create departments.')
          return
        }

        setError('Unable to create department.')
        return
      }

      navigate('/departments')
    } catch {
      setError('Unable to connect to the server.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="container py-4">
      <h2 className="mb-1">Create Department</h2>
      <p className="text-muted mb-4">
        Add a new department.
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

        <div className="mt-4 text-center">
          <button
            type="submit"
            className="btn btn-primary"
            disabled={loading}
          >
            {loading ? 'Creating...' : 'Create Department'}
          </button>

          <button
            type="button"
            className="btn btn-secondary ms-2"
            onClick={() => navigate('/departments')}
            disabled={loading}
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  )
}

export default DepartmentCreatePage