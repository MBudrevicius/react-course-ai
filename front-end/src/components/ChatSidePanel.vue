<script>
import { sendMessage } from '../api/chatAPI';

export default {
  name: 'ChatSidePanel',
  data() {
    return {
      isCollapsed: true,
      userInput: '',
      messages: []
    }
  },
  methods: {
    togglePanel() {
      this.isCollapsed = !this.isCollapsed;
    },
    async handleSubmit() {
      if (!this.userInput.trim()) return;
      const userMessage = this.userInput;
      this.messages.push({ role: 'user', content: userMessage });
      this.userInput = '';

      try {
        const response = await sendMessage({ message: userMessage });
        const assistantMessage = response;
        this.messages.push({ role: 'assistant', content: assistantMessage });
      } catch (error) {
        console.error('Error calling backend:', error);
        this.messages.push({
          role: 'assistant',
          content: 'Sorry, there was an error. Please try again.'
        });
      }
    }
  }
}
</script>


<template>
  <div class="chat-container">
    <button class="chat-toggle-button" @click="togglePanel"  v-if="isCollapsed">
      <span class="chat-icon">💬</span>
    </button>

    <transition name="slide">
      <div v-if="!isCollapsed" class="chat-panel">
        <div class="chat-header">
          <h2>Chat with GPT</h2>
          <button class="close-button" @click="togglePanel">✕</button>
        </div>

        <div class="messages-container">
          <div
            v-for="(msg, index) in messages"
            :key="index"
            :class="['message', msg.role]"
          >
            <strong>{{ msg.role }}:</strong> {{ msg.content }}
          </div>
        </div>

        <form @submit.prevent="handleSubmit" class="chat-form">
          <input
            v-model="userInput"
            type="text"
            class="chat-input"
            placeholder="Type your message..."
          />
          <button type="submit" class="send-button">Send</button>
        </form>
      </div>
    </transition>
  </div>
</template>


<style scoped>
.chat-container {
  position: relative;
  height: 100%; 
}
.chat-toggle-button {
  position: fixed;
  bottom: 20px;
  right: 20px;
  width: 56px;
  height: 56px;
  background-color: #007bff;
  border: none;
  border-radius: 50%;
  color: #fff;
  font-size: 24px;
  cursor: pointer;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
  z-index: 9999;
  display: flex;
  align-items: center;
  justify-content: center;
}

.chat-icon {
  pointer-events: none;
}

.slide-enter-active,
.slide-leave-active {
  transition: transform 0.3s ease, opacity 0.3s ease;
}

.slide-enter-from,
.slide-leave-to {
  opacity: 0;
  transform: translateX(100%);
}

.chat-panel {
  position: fixed;
  top: 60px;
  right: 0;
  width: 320px;
  height: calc(100vh - 60px);
  background-color: #fff;
  border-left: 1px solid #ccc;
  box-shadow: -2px 0 6px rgba(0, 0, 0, 0.15);
  display: flex;
  flex-direction: column;
  z-index: 9998;
}

.chat-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px;
  border-bottom: 1px solid #ccc;
  background-color: #2D2D2D;
  color: white;
}

.close-button {
  background: transparent;
  border: none;
  font-size: 18px;
  cursor: pointer;
  color: #fff;
}

.messages-container {
  flex: 1;
  overflow-y: auto;
  padding: 16px;
  background-color: #2D2D2D;

}

.message {
  margin-bottom: 8px;
}
.message.user {
  text-align: right;
  color: #ffffff;
  border: 1px solid #ccc;
  border-radius: 10px;
  background-color: #4E4E4E;
  padding: 8px;
}
.message.assistant {
  text-align: left;
  color: #fff;
  border: 2px solid #916ad5;
  border-radius: 10px;
  background-color: #4E4E4E;
  padding: 8px;
}

.chat-form {
  display: flex;
  padding: 16px;
  border-top: 1px solid #ccc;
  background-color: #2D2D2D;
}

.chat-input {
  flex: 1;
  padding: 8px;
  font-size: 14px;
  border-radius: 5px;
}

.send-button {
  margin-left: 8px;
  padding: 8px 16px;
  cursor: pointer;
  color: white;
  background-color: #4E4E4E;
  border-radius: 10px;
}
</style>
