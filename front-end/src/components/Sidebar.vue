<script setup>
import { ref, onMounted } from 'vue'
import { getLessonsTitles } from '../api/lessonAPI'
import { isUserLoggedIn } from '../api/user'

const loggedIn = ref(false)
const lessons = ref([])

onMounted(async () => {
  try {
    const userLoggedIn = await isUserLoggedIn()
    if (userLoggedIn) {
      loggedIn.value = true
      const data = await getLessonsTitles()
      lessons.value = data || []
      console.log('Lessons:', lessons.value)
    }
  } catch (error) {
    console.log('Error checking user login status:', error);
    loggedIn.value = false;
  }
})
</script>

<template>
  <div v-if="loggedIn" class="sidebar-container">
    <div class="sidebar">
      <div class="sidebar-item">
        <img src="/svg/book.svg" class="svg" />
      </div>
      <ul>
        <li v-for="(lesson, index) in lessons" :key="lesson.id">
          <router-link :to="`/lessons/${lesson.id}`" class="lesson-link">
            {{ index + 1 }}. {{ lesson.title }}
          </router-link>
        </li>
      </ul>
    </div>
  </div>
</template>

<style scoped>
.svg {
  width: 30px;
  height: 30px;
  filter: invert(47%) sepia(72%) saturate(0%)  brightness(88%) contrast(89%);
  margin-left: 5px;
}

ul {
  padding: 10px;
}

li {
  padding: 3px;
}

li:hover {
  background-color: #49494948;
  border-radius: 10px;
}

.sidebar-container {
  position: relative;
  height: 100%;
}

.sidebar {
  background-color: #333333;
  font-size: 28px;
  width: 100%;
  transition: width 0.4s;
  box-shadow: 3px 0 10px rgba(0, 0, 0, 0.4);
  height: 100%;
  min-height: calc(100vh - 67px);
}

a {
  color: white;
  display: block;
  font-size: 20px;
}

.rounded-rectangle {
  border-radius: 20px;
  background: #4E4E4E;
}
.sidebar-item {
  padding-top: 10px;
  display: flex;
  align-items: center;
  align-content: center;
  justify-content: center;
}

.sidebar-item:hover {
  text-decoration: none;
}

</style>