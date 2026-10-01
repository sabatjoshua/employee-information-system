const authService = {
    login(data) {
    localStorage.setItem('token', data.token)
    localStorage.setItem('userId', data.userId)
    localStorage.setItem('employeeId', data.employeeId)
    localStorage.setItem('userName', data.userName)
    },
    getToken() {
    return localStorage.getItem('token')
    },

    isAuthenticated() {
    return !!localStorage.getItem('token')
    },

    getUserName() {
    return localStorage.getItem('userName')
    },

    getUserId() {
    return localStorage.getItem('userId')
    },

    getEmployeeId() {
    return localStorage.getItem('employeeId')
    },

    logout() {
    localStorage.removeItem('token')
    localStorage.removeItem('userId')
    localStorage.removeItem('employeeId')
    localStorage.removeItem('userName')
    },
}

export default authService