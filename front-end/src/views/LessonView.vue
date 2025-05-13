<script setup>
import Navbar from '../components/Navbar.vue'
import SideBar from '../components/Sidebar.vue'
import Task from '@/components/Task.vue';
import { ref, onMounted, onUnmounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { getLessonById, getTasksByLessonId } from '../api/lessonAPI'
import ChatSidePanel from '@/components/ChatSidePanel.vue';
import Tutorial from '@/components/Tutorial.vue';

const route = useRoute()
const lessonId = ref(route.params.id)
const lessonTitle = ref('')
const lessonContent = ref('')
const taskContent = ref('No tasks available')
const showTutorial = ref(false);
const sidebarOpen = ref(true);

watch(() => route.params.id, async (newId) => {
  lessonId.value = newId;
  fetchLesson();
  fetchTasks();
});

onMounted(async () => {
  fetchLesson();
  fetchTasks();
  handleResize();
  window.addEventListener('resize', handleResize);

  const tutorialCompleted = localStorage.getItem('tutorialCompleted');
  if (!tutorialCompleted) {
    setTimeout(() => {
      showTutorial.value = true;
    }, 1000);
  }
})

onUnmounted(() => {
  window.removeEventListener('resize', handleResize);
})

const handleResize = () => {
  if (window.innerWidth > 960) {
    sidebarOpen.value = true;
  } 
  else if (window.innerWidth <= 768) {
    sidebarOpen.value = false;
  }
};

function closeTutorial() {
  showTutorial.value = false;
  localStorage.setItem('tutorialCompleted', 'true');
}

function toggleSidebar() {
  sidebarOpen.value = !sidebarOpen.value;
}

async function fetchLesson() {
  try {
    const lesson = await getLessonById(lessonId.value);
    lessonTitle.value = lesson.title;
    lessonContent.value = lesson.content;
  } catch (error) {
    console.log('Error fetching lesson:', error);
  }
}

async function fetchTasks() {
  try {
    const tasks = await getTasksByLessonId(lessonId.value);
    taskContent.value = tasks.length > 0 ? tasks[0].question : 'No tasks available';
  } catch (error) {
    console.log('Error fetching tasks:', error);
    taskContent.value = 'Error fetching tasks';
  }
}
</script>

<template>
  <Navbar />
  <div v-if="!sidebarOpen" class="sidebar-toggle closed" @click="toggleSidebar">
    <img src="/svg/arrow-right.svg" alt="Open menu" class="toggle-icon" />
  </div>
  
  <div class="grid-container" :class="{ 'sidebar-collapsed': !sidebarOpen }">
    <div class="sidebar" :class="{ 'hidden': !sidebarOpen }">
      <SideBar />
    </div>
    
    <div v-if="sidebarOpen" class="sidebar-toggle open" @click="toggleSidebar">
      <img src="/svg/arrow-left.svg" alt="Close menu" class="toggle-icon" />
    </div>
        
    <div class="content">
      <Task :taskContent="taskContent" />      
      <h1 v-if="lessonTitle" class="theory">{{ lessonTitle }}</h1>
      <h1 v-else class="theory">Labas!</h1>
      <p v-if="lessonTitle" class="theory" v-html="lessonContent"></p>   
      <p v-else class="theory">Jei nori pradėti mokytis, pasirink pamoką iš šoninės juostos.</p>   
    </div>
    <div class="chat">
      <ChatSidePanel :lessonId="lessonId" :lessonTitle="lessonTitle" />
    </div>
  </div>  
  <Tutorial :isVisible="showTutorial" @close="closeTutorial" />
</template>

<style scoped>
.sidebar-toggle {
  position: fixed;
  top: 130px;
  width: 30px;
  height: 30px;
  background-color: #333333;
  display: none;
  justify-content: center;
  align-items: center;
  cursor: pointer;
  z-index: 1000;
  box-shadow: 2px 0 5px rgba(0, 0, 0, 0.2);
}

.sidebar-toggle.open {
  left: 250px;
  border-radius: 0 10px 10px 0;
}

.sidebar-toggle.closed {
  left: 0;
  border-radius: 0 5px 5px 0;
}

.toggle-icon {
  width: 16px;
  height: 16px;
  filter: invert(1);
}

.grid-container {
  display: grid;
  grid-template-areas: "sidebar content";
  grid-template-columns: 1fr 5fr;
  transition: all 0.3s ease;
  min-height: calc(100vh - 65px);
}

.grid-container.sidebar-collapsed {
  grid-template-columns: 0fr 1fr;
}

.sidebar {
  grid-area: sidebar;
  height: 100%;
  min-height: calc(100vh - 65px);
  transition: all 0.3s ease;
  overflow: hidden;
  position: relative;
  background-color: #333333;
}

.sidebar.hidden {
  width: 0;
  overflow: hidden;
}

.content {
  grid-area: content;
  display: flex;
  flex-direction: column;
  margin-left: 40px;
  margin-right: 40px;
}

.chat {
  grid-area: chat;
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
}

@media (max-width: 960px) {
  .sidebar-toggle {
    display: flex;
  }
  
  .sidebar-toggle.open {
    right: 0;
  }
  
  .grid-container {
    grid-template-columns: auto 1fr;
  }
  
  .grid-container.sidebar-collapsed {
    grid-template-columns: 0 1fr;
  }
  
  .sidebar {
    position: relative;
    left: 0;
    bottom: 0;
    z-index: 900;
    width: 250px;
    height: 100%;
  }
  
  .sidebar.hidden {
    left: -250px;
  }
  
  .content {
    grid-column: 1 / -1;
    margin-left: 20px;
    margin-right: 20px;
  }
}
@media (max-width: 768px) {
  h1.theory {
    font-size: 28px;
  }
  
  p.theory {
    font-size: 18px;
    line-height: 1.4;
    text-align: left;
  }
  
  .theory pre {
    font-size: 15px;
  }
}
@media (max-width: 480px) {
  .sidebar {
    width: 200px;
  }
  
  .sidebar.hidden {
    left: -200px;
  }
  
  .content {
    margin-left: 10px;
    margin-right: 10px;
  }
  
  h1.theory {
    font-size: 24px;
    margin-top: 20px;
  }
  
  p.theory {
    font-size: 16px;
    text-align: left;
  }
  
  .theory li {
    font-size: 16px;
  }
  
  .theory pre {
    font-size: 14px;
    padding: 8px;
  }
  
  .task {
    margin-bottom: 10px;
  }
}

</style>

<style>
h1.theory {
  color: white;
  font-size: 40px;
  margin-top: 40px;
  text-align: center;
}

p.theory {
  color: white;
  margin-bottom: 20px;
  font-size: 20px;
  text-align: justify;
}

.theory ol {
  margin-left: 20px;
  padding-left: 20px;
  list-style-type: decimal;
}

.theory li {
  margin-bottom: 10px;
  font-size: 18px;
  color: white;
}

.theory b {
  font-weight: bold;
}

.theory pre {
  background-color: #2d2d2d;
  color: #f8f8f2;
  padding: 10px;
  border-radius: 5px;
  overflow-x: auto;
  font-family: 'Courier New', Courier, monospace;
  font-size: 16px;
  margin: 20px 0;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.3);
  border-left: 3px solid #916ad5;
  max-width: 100%;
}

.theory code {
  font-family: 'Courier New', Courier, monospace;
  color: #f8f8f2;
  background-color: #3d3d3d;
  padding: 2px 5px;
  border-radius: 3px;
  font-size: 0.9em;
}
.theory table {
  border-collapse: collapse;
  width: 100%;
  margin: 20px 0;
  color: white;
  background-color: #2d2d2d;
  border-radius: 5px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.3);
}

.theory th {
  font-size: 18px;
  background-color: #3d3d3d;
  text-align: left;
  padding: 12px;
  font-weight: bold;
  border-bottom: 2px solid #916ad5;
}

.theory td {
  font-size: 16px;
  padding: 10px 12px;
  border-bottom: 1px solid #444;
}

.theory tr:last-child td {
  border-bottom: none;
}

.theory td:hover, .theory th:hover {
  background-color: #3a3a3a;
}

@media (max-width: 960px) {
  .theory pre {
    font-size: 15px;
    padding: 10px;
    margin: 16px 0;
  }
  .theory table {
    font-size: 15px;
  }
  
  .theory th, .theory td {
    padding: 10px;
  }
}

@media (max-width: 768px) {
  .theory pre {
    font-size: 14px;
    padding: 8px;
    border-radius: 4px;
    margin: 14px 0;
  }
  
  .theory code {
    font-size: 0.85em;
    padding: 1px 4px;
  }
  .theory table {
    font-size: 14px;
  }
  
  .theory th, .theory td {
    padding: 8px;
  }
}

@media (max-width: 480px) {
  .theory pre {
    font-size: 13px;
    padding: 6px;
    border-radius: 3px;
    border-left: 2px solid #916ad5;
    margin: 12px 0;
    line-height: 1.4;
  }
  
  .theory code {
    padding: 1px 3px;
  }
  
  .theory pre::-webkit-scrollbar {
    height: 4px;
  }
  
  .theory pre::-webkit-scrollbar-thumb {
    background-color: #916ad5;
    border-radius: 2px;
  }
  
  .theory pre::-webkit-scrollbar-track {
    background-color: #3d3d3d;
  }
}
</style>