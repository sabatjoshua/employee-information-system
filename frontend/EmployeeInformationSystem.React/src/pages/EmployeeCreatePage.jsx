import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import API_BASE_URL from '../services/api'

function EmployeeCreatePage() {
  const navigate = useNavigate()

  const [departments, setDepartments] = useState([])
  const [selectedDepartmentId, setSelectedDepartmentId] = useState('')
  const [positions, setPositions] = useState([])

  const [form, setForm] = useState({
    employeeNo: '',
    firstName: '',
    middleName: '',
    lastName: '',
    genderCode: '',
    birthDate: '',
    email: '',
    mobileNo: '',
    hireDate: '',
    departmentId: '',
    positionId: '',
  })

  useEffect(() => {
    const loadDepartments = async () => {
      try {
        const token = localStorage.getItem('token')

        const response = await fetch(
          `${API_BASE_URL}/api/Departments`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        )

        if (!response.ok) {
          throw new Error('Failed to load departments.')
        }

        const data = await response.json()

        setDepartments(data)
      } catch (error) {
        console.error(error)
      }
    }

    loadDepartments()
  }, [])

  useEffect(() => {
    const loadPositions = async () => {
      if (!selectedDepartmentId) {
        setPositions([])
        return
      }

      try {
        const token = localStorage.getItem('token')

        const response = await fetch(
          `${API_BASE_URL}/api/Positions?departmentId=${selectedDepartmentId}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        )

        if (!response.ok) {
          throw new Error('Failed to load positions.')
        }

        const data = await response.json()

        setPositions(data)
      } catch (error) {
        console.error(error)
        setPositions([])
      }
    }

    loadPositions()
  }, [selectedDepartmentId])

  const handleChange = (event) => {
    const { name, value } = event.target

    setForm((current) => ({
      ...current,
      [name]: value,
    }))
  }

  const handleDepartmentChange = (event) => {
    const departmentId = event.target.value

    setSelectedDepartmentId(departmentId)

    setForm((current) => ({
      ...current,
      departmentId,
      positionId: '',
    }))
  }
    const handleSubmit = async (event) => {
    event.preventDefault()

    try {
        const token = localStorage.getItem('token')

        const response = await fetch(
        `${API_BASE_URL}/api/Employees`,
        {
            method: 'POST',
            headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({
            employeeNo: form.employeeNo,
            firstName: form.firstName,
            middleName: form.middleName || null,
            lastName: form.lastName,
            genderCode: form.genderCode,
            birthDate: form.birthDate,
            email: form.email || null,
            mobileNo: form.mobileNo || null,
            hireDate: form.hireDate,
            departmentId: form.departmentId,
            positionId: form.positionId,
            }),
        }
        )

        if (!response.ok) {
        const errorText = await response.text()

        throw new Error(
            errorText || 'Failed to create employee.'
        )
        }

        const data = await response.json()

        console.log('Employee created:', data)

        navigate('/employees')
    } catch (error) {
        console.error(error)
    }
    }
  return (
    <div className="container py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>Create Employee</h2>

        <button
          type="button"
          className="btn btn-secondary"
          onClick={() => navigate('/employees')}
        >
          Back to Employees
        </button>
      </div>

      <div className="card shadow-sm">
        <div className="card-body">
            <form onSubmit={handleSubmit}>
            <div className="row">

                {/* ==================== LEFT COLUMN ==================== */}
                <div className="col-md-6">

                {/* Employee No */}
                <div className="mb-3">
                    <div className="row align-items-center">
                    <label
                        htmlFor="employeeNo"
                        className="col-sm-4 col-form-label"
                    >
                        Employee No
                    </label>

                    <div className="col-sm-8">
                        <input
                        type="text"
                        className="form-control"
                        id="employeeNo"
                        name="employeeNo"
                        value={form.employeeNo}
                        onChange={handleChange}
                        />
                    </div>
                    </div>
                </div>

                {/* First Name */}
                <div className="mb-3">
                    <div className="row align-items-center">
                    <label
                        htmlFor="firstName"
                        className="col-sm-4 col-form-label"
                    >
                        First Name
                    </label>

                    <div className="col-sm-8">
                        <input
                        type="text"
                        className="form-control"
                        id="firstName"
                        name="firstName"
                        value={form.firstName}
                        onChange={handleChange}
                        />
                    </div>
                    </div>
                </div>

                {/* Middle Name */}
                <div className="mb-3">
                    <div className="row align-items-center">
                    <label
                        htmlFor="middleName"
                        className="col-sm-4 col-form-label"
                    >
                        Middle Name
                    </label>

                    <div className="col-sm-8">
                        <input
                        type="text"
                        className="form-control"
                        id="middleName"
                        name="middleName"
                        value={form.middleName}
                        onChange={handleChange}
                        />
                    </div>
                    </div>
                </div>

                {/* Last Name */}
                <div className="mb-3">
                    <div className="row align-items-center">
                    <label
                        htmlFor="lastName"
                        className="col-sm-4 col-form-label"
                    >
                        Last Name
                    </label>

                    <div className="col-sm-8">
                        <input
                        type="text"
                        className="form-control"
                        id="lastName"
                        name="lastName"
                        value={form.lastName}
                        onChange={handleChange}
                        />
                    </div>
                    </div>
                </div>

                {/* Hire Date */}
                <div className="mb-3">
                    <div className="row align-items-center">
                    <label
                        htmlFor="hireDate"
                        className="col-sm-4 col-form-label"
                    >
                        Hire Date
                    </label>

                    <div className="col-sm-8">
                        <input
                        type="date"
                        className="form-control"
                        id="hireDate"
                        name="hireDate"
                        value={form.hireDate}
                        onChange={handleChange}
                        />
                    </div>
                    </div>
                </div>

                {/* Gender */}
                <div className="mb-3">
                    <div className="row align-items-center">
                    <label
                        htmlFor="genderCode"
                        className="col-sm-4 col-form-label"
                    >
                        Gender
                    </label>

                    <div className="col-sm-8">
                        <select
                        className="form-select"
                        id="genderCode"
                        name="genderCode"
                        value={form.genderCode}
                        onChange={handleChange}
                        >
                        <option value="">Select Gender</option>
                        <option value="M">Male</option>
                        <option value="F">Female</option>
                        </select>
                    </div>
                    </div>
                </div>

                </div>

                {/* ==================== RIGHT COLUMN ==================== */}
                <div className="col-md-6">

                {/* Birth Date */}
                <div className="mb-3">
                    <div className="row align-items-center">
                    <label
                        htmlFor="birthDate"
                        className="col-sm-4 col-form-label"
                    >
                        Birth Date
                    </label>

                    <div className="col-sm-8">
                        <input
                        type="date"
                        className="form-control"
                        id="birthDate"
                        name="birthDate"
                        value={form.birthDate}
                        onChange={handleChange}
                        />
                    </div>
                    </div>
                </div>

                {/* Email */}
                <div className="mb-3">
                    <div className="row align-items-center">
                    <label
                        htmlFor="email"
                        className="col-sm-4 col-form-label"
                    >
                        Email
                    </label>

                    <div className="col-sm-8">
                        <input
                        type="email"
                        className="form-control"
                        id="email"
                        name="email"
                        value={form.email}
                        onChange={handleChange}
                        />
                    </div>
                    </div>
                </div>

                {/* Mobile No */}
                <div className="mb-3">
                    <div className="row align-items-center">
                    <label
                        htmlFor="mobileNo"
                        className="col-sm-4 col-form-label"
                    >
                        Mobile No
                    </label>

                    <div className="col-sm-8">
                        <input
                        type="text"
                        className="form-control"
                        id="mobileNo"
                        name="mobileNo"
                        value={form.mobileNo}
                        onChange={handleChange}
                        />
                    </div>
                    </div>
                </div>

                {/* Department */}
                <div className="mb-3">
                    <div className="row align-items-center">
                    <label
                        htmlFor="departmentId"
                        className="col-sm-4 col-form-label"
                    >
                        Department
                    </label>

                    <div className="col-sm-8">
                        <select
                        className="form-select"
                        id="departmentId"
                        name="departmentId"
                        value={form.departmentId}
                        onChange={handleDepartmentChange}
                        >
                        <option value="">
                            Select Department
                        </option>

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
                </div>

                {/* Position */}
                <div className="mb-3">
                    <div className="row align-items-center">
                    <label
                        htmlFor="positionId"
                        className="col-sm-4 col-form-label"
                    >
                        Position
                    </label>

                    <div className="col-sm-8">
                        <select
                        className="form-select"
                        id="positionId"
                        name="positionId"
                        value={form.positionId}
                        onChange={handleChange}
                        disabled={!selectedDepartmentId}
                        >
                        <option value="">
                            {selectedDepartmentId
                            ? 'Select Position'
                            : 'Select Department first'}
                        </option>

                        {positions.map((position) => (
                            <option
                            key={position.id}
                            value={position.id}
                            >
                            {position.name}
                            </option>
                        ))}
                        </select>
                    </div>
                    </div>
                </div>

                </div>
            </div>

            <div className="mt-4 text-center">
            <button
                type="submit"
                className="btn btn-primary px-4"
            >
                Create Employee
            </button>

            <button
                type="button"
                className="btn btn-secondary ms-2"
                onClick={() => navigate('/employees')}
            >
                Cancel
            </button>
            </div>
            
            </form>
        </div>
      </div>
    </div>
  )
}

export default EmployeeCreatePage