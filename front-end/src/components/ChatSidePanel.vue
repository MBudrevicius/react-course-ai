<script>
import { sendMessage } from '../api/chatAPI';

export default {
  name: 'ChatSidePanel',
  data() {
    return {
      isCollapsed: true,
      userInput: '',
      messages: [],
      contextId: null
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
        const response = await sendMessage({
          message: userMessage,
          contextId: this.contextId
        });
        console.log("aaaa:", response);
        this.messages.push({ role: 'assistant', content: response.reply });
        if (response.contextId) {
          this.contextId = response.contextId;
        }
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
       <img src="/svg/chat.svg" class="svg"/>
    </button>

    <transition name="slide">
      <div v-if="!isCollapsed" class="chat-panel">
        <div class="chat-header">
          <div style="display: flex; align-items: center;">
            <div class="img-circle">
              <img src="/images/stand.png">
            </div>
            <h2>Komponionas</h2>
          </div>
          <button class="close-button" @click="togglePanel">✕</button>
        </div>

        <div class="messages-container">
          <div
            v-for="(msg, index) in messages"
            :key="index"
            :class="['message', msg.role]"
          >
           {{ msg.content }}
          </div>
        </div>

        <form @submit.prevent="handleSubmit" class="chat-form">
          <input
            v-model="userInput"
            type="text"
            class="chat-input"
            placeholder="Reikia pagalbos?"
          />
          <button type="submit" class="send-button"><img src="/svg/mic.svg" class="svg" style="width: 25px; height: 25px;"/></button>
          <button type="submit" class="send-button"><img src="/svg/send.svg" class="svg" style="width: 30px; height: 30px;"/></button>
        </form>
      </div>
    </transition>
  </div>
</template>


<style scoped>
.chat-header h2 {
  font-size: 20px;
  color: white;
  margin-left: 10px;
}

.img-circle {
    width: 40px;
    height: 40px;
    border-radius: 100%; 
    overflow: hidden; 
    display: flex; 
    align-items: center; 
    justify-content: center; 
}

.img-circle img {
    width: auto;
    height: 90px; 
    margin-top: 35px;
    object-fit: cover; 
    background-color: white;
}

.svg {
  width: 30px;
  height: 30px;
  filter: invert(1);
}

.chat-container {
  position: sticky;
  bottom: 0;
  height: 100%;
}

.chat-toggle-button {
  position: fixed;
  bottom: 20px;
  right: 20px;
  width: 56px;
  height: 56px;
  background-color: #916ad5;
  border-radius: 50%;
  cursor: help;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
  display: flex;
  align-items: center;
  justify-content: center;
}

.slide-enter-active,
.slide-leave-active {
  transition: transform 0.3s ease, opacity 0.3s ease;
}

.slide-enter-from,
.slide-leave-to {
  opacity: 0;
  transform: translateY(100%);
}

.chat-panel {
  /* position: fixed;
  top: 0;
  height: 100%;
  top: 67px;
  right: 0;
  width: 320px; */
  /* height: calc(100vh - 60px); */
  /* background-color: #fff;
  border-left: 1px solid #ccc;
  box-shadow: -2px 0 6px rgba(0, 0, 0, 0.15);
  display: flex;
  flex-direction: column; */
  /* z-index: 9998; */
  position: fixed;
  bottom: 0;
  min-width: 300px;
}

.chat-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px;
  background-color: #2D2D2D;
  color: white;
  border-top-left-radius: 10px;
  border-top-right-radius: 10px;
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
  background-color: #4E4E4E;
  height: 500px;
  box-shadow:rgba(0, 0, 0, 0.15) 0px 1px 3px 0px inset;
}

.message {
  margin-bottom: 8px;
}

.message.user {
  text-align: right;
  color: black;
  background-color: white;
  padding: 8px;
  border-top-left-radius: 15px;
  border-top-right-radius: 15px;
  border-bottom-left-radius: 15px;
  margin-left: 20px;
}

.message.assistant {
  text-align: left;
  color: white;
  background-color: #916ad5;
  padding: 8px;
  border-top-left-radius: 15px;
  border-top-right-radius: 15px;
  border-bottom-right-radius: 15px;
  margin-right: 20px;
}

.chat-form {
  display: flex;
  padding: 16px;
  background-color: #2D2D2D;
}

.chat-input {
  flex: 1;
  padding: 5px;
  padding-left: 10px;
  font-size: 14px;
  border-radius: 20px;
}

.send-button {
  margin-left: 8px;
  cursor: pointer;
  filter: invert(47%) sepia(72%) saturate(467%) hue-rotate(219deg) brightness(88%) contrast(89%);
}
</style>
