import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import authService from '../services/authService'
import API_BASE_URL from '../services/api'

function LoginPage() {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const navigate = useNavigate()
return (
  <div className="container d-flex justify-content-center align-items-center min-vh-100">
    <div className="card shadow-sm w-100" style={{ maxWidth: '400px' }}>
      <div className="card-body p-4">
        <h2 className="text-center mb-4">
          Employee Information System
        </h2>
        <p className="text-center text-muted mb-4">
          Sign in to continue
        </p>

        <h5 className="text-center mb-4">Login</h5>

        {error && (
          <div className="alert alert-danger" role="alert">
            {error}
          </div>
        )}

      <form
        onSubmit={async (e) => {
            e.preventDefault()
            setError('')

            if (!username.trim() || !password.trim()) {
            setError('Username and password are required.')
            return
            }

           try {
            const response = await fetch(
                `${API_BASE_URL}/api/Authentication/login`,
                {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    userName: username,
                    password: password,
                }),
                },
            )

            if (!response.ok) {
                if (response.status === 401) {
                setError('Invalid username or password.')
                return
                }

                setError('Unable to login. Please try again.')
                return
            }

            const data = await response.json()

            authService.login(data)
            navigate('/')
            } catch {
            setError('Unable to connect to the server. Please try again.')
            }
        }}
        >
        <div className="mb-3">
          <label htmlFor="username" className="form-label">
            Username
          </label>

          <input
            id="username"
            type="text"
            className="form-control"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label htmlFor="password" className="form-label">
            Password
          </label>

          <input
            id="password"
            type="password"
            className="form-control"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>

        <button type="submit" className="btn btn-primary w-100">
          Login
        </button>
      </form></div>
    </div>
  </div>
)
}

export default LoginPage