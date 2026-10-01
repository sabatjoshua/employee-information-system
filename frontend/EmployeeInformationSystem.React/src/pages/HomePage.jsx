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
    <div>
      <h1>Employee Information System</h1>

      <h2>Welcome, {userName}! 👋</h2>

      <button onClick={handleLogout}>
        Logout
      </button>
    </div>
  )
}

export default HomePage