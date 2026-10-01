import { useNavigate } from 'react-router-dom'
import authService from '../services/authService'

function HomePage() {
  const navigate = useNavigate()
  const userName = authService.getUserName()

  const handleLogout = () => {
    authService.logout()
    navigate('/login')
  }

  return (
    <>
      <nav className="navbar navbar-dark bg-dark">
        <div className="container">
          <span className="navbar-brand">
            Employee Information System
          </span>

          <button
            type="button"
            className="btn btn-outline-light btn-sm"
            onClick={handleLogout}
          >
            Logout
          </button>
        </div>
      </nav>

      <main className="container py-4">
        <h2>Welcome, {userName}! 👋</h2>

        <p className="text-muted">
          Welcome to the Employee Information System.
        </p>

        <div className="row mt-4">
          <div className="col-md-4">
            <div className="card shadow-sm">
              <div className="card-body">
                <h5 className="card-title">Employees</h5>

                <p className="card-text text-muted">
                  Manage employee information.
                </p>

                <button
                  type="button"
                  className="btn btn-primary"
                  onClick={() => navigate('/employees')}
                >
                  View Employees
                </button>
              </div>
            </div>
          </div>
        </div>
      </main>
    </>
  )
}

export default HomePage