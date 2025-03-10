<script setup>
import { ref, onMounted } from 'vue'
import { getLessonsTitles } from '../api/lessonAPI'
import { isUserLoggedIn } from '../api/user'
import { FwbSidebar, FwbSidebarItem, FwbSidebarDropdownItem } from 'flowbite-vue'

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
    <fwb-sidebar class="sidebar">
      <fwb-sidebar-item class="sidebar-item">
        <template #icon>
          <svg
              class="flex-shrink-0 w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="currentColor"
              viewBox="0 0 16 20"
          >
            <path d="M16 14V2a2 2 0 0 0-2-2H2a2 2 0 0 0-2 2v15a3 3 0 0 0 3 3h12a1 1 0 0 0 0-2h-1v-2a2 2 0 0 0 2-2ZM4 2h2v12H4V2Zm8 16H3a1 1 0 0 1 0-2h9v2Z" />
          </svg>
        </template>
        <template #default>Pamokos</template>
      </fwb-sidebar-item>
      <ul>
        <li v-for="(lesson, index) in lessons" :key="lesson.id">
          <router-link :to="`/lessons/${lesson.id}`" class="lesson-link">
            {{ index + 1 }}. {{ lesson.title }}
          </router-link>
        </li>
      </ul>
    </fwb-sidebar>

  </div>
</template>

<style scoped>
.sidebar-container {
  position: relative;
}

.sidebar {
  /* Neveikia spalva. */
  background-color: #2D2D2D;
  font-size: 28px;
  /* Užkomentuoju, nes overlayina net navbarą */
  /* position: fixed; */
  width: 250px;
  transition: width 0.4s;
  overflow: hidden;
}

.sidebar.collapsed {
  width: 0;
}

.sidebar-elements {
  display: flex;
  float: right;
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

.toggle-button {
  background-color: #916ad5;
  color: white;
  padding: 10px;
  cursor: pointer;
  position: fixed;
  top: 100px;
  left: 250px;
  width: 30px;
  height: 30px;
  border-top-right-radius: 10px;
  border-bottom-right-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: left 0.4s;
}

.toggle-button.collapsed {
  left: 0px; 
}

.sidebar-item {
  display: flex;
  align-items: center;
}

.sidebar-item:hover {
  text-decoration: none;
}

.lesson-link:hover {
  color: #916ad5;
}
</style>