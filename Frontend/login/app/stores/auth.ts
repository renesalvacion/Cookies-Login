import { defineStore } from 'pinia'

interface UserSession {
  userId: string
  email: string
  name: string
}

interface UserData {
  id: number
  firstname: string
  lastname: string
  email: string
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null as UserSession | null,
    loading: false,
    users: [] as UserData[]
  }),

  getters: {
    isAuthenticated: (state) => !!state.user
  },

  actions: {
    async login(email: string, password: string) {
      this.loading = true
      try {
        await $fetch('http://localhost:5295/api/User/login', {
          method: 'POST',
          credentials: 'include',
          body: { email, password }
        })
        await this.fetchSession()
        return true
      } catch (err: any) {
        throw err?.data || 'Login failed'
      } finally {
        this.loading = false
      }
    },

    async fetchSession() {
      try {
        const data = await $fetch<UserSession>(
          'http://localhost:5295/api/User/session',
          { credentials: 'include' }
        )
        this.user = data
        return data
      } catch (err) {
        this.user = null
        return null
      }
    },

    async logout() {
      this.user = null
      await $fetch('http://localhost:5295/api/User/logout', {
        method: 'POST',
        credentials: 'include'
      })
    },

    // ✅ New function to fetch all users
    async getUserDataChat() {
      this.loading = true
      try {
        const data = await $fetch<UserData[]>(
          'http://localhost:5295/api/User/users',
          { credentials: 'include' }
        )
        this.users = data
        return data
      } catch (err) {
        console.error('Error fetching users:', err)
        this.users = []
        return []
      } finally {
        this.loading = false
      }
    }
  }
})
