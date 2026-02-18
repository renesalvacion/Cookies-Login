<template>
  <h1>Dashboard</h1>

  <!-- Users List -->
  <div v-if="users.length">
    <h2>Chat Partners</h2>
    <ul>
      <li v-for="user in users" :key="user.userid" class="mb-2 flex items-center justify-between">
        <span>{{ user.firstname }} {{ user.lastname }} ({{ user.email }})</span>
        <button
          class="px-3 py-1 bg-blue-500 text-white rounded hover:bg-blue-600"
          @click="openChat(user.userid)"
        >
          Chat
        </button>
      </li>
    </ul>
  </div>

  <div v-else>
    <p>No users found</p>
  </div>

  <!-- Chat Modal -->
      <MessengerModal
  v-for="(chat, index) in messengerStore.openChats.filter(c => c.isOpen)"
  :key="chat.partnerId + '-' + chat.page"
  :chat="chat"
  :index="index"
  @close="messengerStore.closeChat"
/>

</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import MessengerModal from '~/components/MessengerModal.vue'
import { useAuthStore } from '#imports'
import { useMessengerStore } from '#imports'

interface UserData {
  userid: number
  firstname: string
  lastname: string
  email: string
}

const sessionStore = useAuthStore()
const messengerStore = useMessengerStore()

const users = ref<UserData[]>([])
const isChatModal = ref(false)
const selectedPartnerId = ref<number | null>(null)

// Fetch users from API
const fetchUsers = async () => {
  const data = await sessionStore.getUserDataChat()
  users.value = data
}

// Open chat modal with selected user
const openChat = async (partnerId: number) => {
  selectedPartnerId.value = partnerId

  // Optional: fetch chat messages
  if (sessionStore.user?.userId) {
    await messengerStore.viewMessagePerson(Number(sessionStore.user.userId), partnerId)
  }

  isChatModal.value = true
}

onMounted(async () => {
  await sessionStore.fetchSession()
  await fetchUsers()
})
</script>

<style scoped>
ul {
  list-style: none;
  padding: 0;
}

li {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>
