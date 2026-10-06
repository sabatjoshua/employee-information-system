import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import ProtectedRoute from './components/ProtectedRoute'
import HomePage from './pages/HomePage'
import LoginPage from './pages/LoginPage'
import EmployeesPage from './pages/EmployeesPage'
import EmployeeDetailsPage from './pages/EmployeeDetailsPage'
import EmployeeCreatePage from './pages/EmployeeCreatePage'
import EmployeeEditPage from './pages/EmployeeEditPage'

import DepartmentsPage from './pages/DepartmentsPage'
import DepartmentCreatePage from './pages/DepartmentCreatePage'
import DepartmentEditPage from './pages/DepartmentEditPage'

import PositionsPage from './pages/PositionsPage'
import PositionCreatePage from './pages/PositionCreatePage'
import PositionEditPage from './pages/PositionEditPage'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />

        <Route
          path="/"
          element={
            <ProtectedRoute>
              <HomePage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/employees"
          element={
            <ProtectedRoute>
              <EmployeesPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/employees/:id"
          element={
            <ProtectedRoute>
              <EmployeeDetailsPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/employees/:id/edit"
          element={
            <ProtectedRoute>
              <EmployeeEditPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/employees/create"
          element={
            <ProtectedRoute>
              <EmployeeCreatePage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/departments"
          element={
            <ProtectedRoute>
              <DepartmentsPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/departments/create"
          element={
            <ProtectedRoute>
              <DepartmentCreatePage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/departments/:id/edit"
          element={
            <ProtectedRoute>
              <DepartmentEditPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/positions"
          element={
            <ProtectedRoute>
              <PositionsPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/positions/create"
          element={
            <ProtectedRoute>
              <PositionCreatePage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/positions/:id/edit"
          element={
            <ProtectedRoute>
              <PositionEditPage />
            </ProtectedRoute>
          }
        />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App