import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import authService from '../services/authService'
import API_BASE_URL from '../services/api'


function EmployeeEditPage() {
    const navigate = useNavigate()
    const { id } = useParams()

    const [loading, setLoading] = useState(true)
    const [error, setError] = useState('')

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

    const handleChange = (event) => {
        const { name, value } = event.target

        setForm((previous) => ({
            ...previous,
            [name]: value,
        }))
    }

    useEffect(() => {
    const loadEmployee = async () => {
        try {
        const response = await fetch(
            `${API_BASE_URL}/api/Employees/${id}`,
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
            setError('You do not have permission to view this employee.')
            return
            }

            if (response.status === 404) {
            setError('Employee not found.')
            return
            }

            setError('Unable to load employee.')
            return
        }

        const data = await response.json()

        setForm({
            employeeNo: data.employeeNo,
            firstName: data.firstName,
            middleName: data.middleName ?? '',
            lastName: data.lastName,
            genderCode: data.genderCode,
            birthDate: data.birthDate
            ? data.birthDate.slice(0, 10)
            : '',
            email: data.email ?? '',
            mobileNo: data.mobileNo ?? '',
            hireDate: data.hireDate
            ? data.hireDate.slice(0, 10)
            : '',
            departmentId: data.departmentId,
            positionId: data.positionId,
        })

        setSelectedDepartmentId(data.departmentId)
        } catch {
        setError('Unable to connect to the server.')
        } finally {
        setLoading(false)
        }
    }

    loadEmployee()
    }, [id])

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
  if (!selectedDepartmentId) {
    setPositions([])
    return
  }

  const loadPositions = async () => {
    try {
      const response = await fetch(
        `${API_BASE_URL}/api/Positions?departmentId=${selectedDepartmentId}`,
        {
          headers: {
            Authorization: `Bearer ${authService.getToken()}`,
          },
        },
      )

      if (!response.ok) {
        setError('Unable to load positions.')
        return
      }

      const data = await response.json()
      setPositions(data)
    } catch {
      setError('Unable to connect to the server.')
    }
  }

  loadPositions()
}, [selectedDepartmentId])

const handleDepartmentChange = (event) => {
  const departmentId = event.target.value

  setSelectedDepartmentId(departmentId)

  setForm((previous) => ({
    ...previous,
    departmentId,
    positionId: '',
  }))
}

const handleSubmit = async (event) => {
  event.preventDefault()

  try {
    const response = await fetch(
      `${API_BASE_URL}/api/Employees/${id}`,
      {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${authService.getToken()}`,
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
      },
    )

    if (!response.ok) {
      const errorText = await response.text()
      throw new Error(errorText || 'Failed to update employee.')
    }

    navigate(`/employees/${id}`)
  } catch (error) {
    console.error(error)
    setError(error.message || 'Unable to update employee.')
  }
}

return (
  <div className="container py-4">
    <div className="mb-4">
      <h2 className="mb-1">Edit Employee</h2>
      <p className="text-muted mb-0">
        Update employee information.
      </p>
    </div>

    {loading && <p>Loading employee...</p>}

    {error && (
      <div className="alert alert-danger" role="alert">
        {error}
      </div>
    )}

    {!loading && !error && (
      <form onSubmit={handleSubmit}>
        <div className="row">
          {/* Left Column */}
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

          {/* Right Column */}
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
                    <option value="">Select Position</option>

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
            className="btn btn-primary"
          >
            Save Changes
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
    )}
  </div>
)
}

export default EmployeeEditPage