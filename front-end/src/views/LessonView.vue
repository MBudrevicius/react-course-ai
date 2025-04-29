<script setup>
import Navbar from '../components/Navbar.vue'
import SideBar from '../components/Sidebar.vue'
import Task from '@/components/Task.vue';
import { ref, onMounted, watch } from 'vue'
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

onMounted(async () => {
  fetchLesson();
  fetchTasks();
})

watch(() => route.params.id, async (newId) => {
  lessonId.value = newId;
  fetchLesson();
  fetchTasks();
});

const tutorialCompleted = localStorage.getItem('tutorialCompleted');

if (!tutorialCompleted) {
    setTimeout(() => {
      showTutorial.value = true;
    }, 1000);
  }

function closeTutorial() {
  showTutorial.value = false;
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
  <div class="grid-container">
    <div class="sidebar">
      <SideBar />
    </div>    
    <div class="content">
      <Task :taskContent="taskContent" />      
      <h1 v-if="lessonTitle" class="theory">{{ lessonTitle }}</h1>
      <h1 v-else class="theory">Labas!</h1>
      <p v-if="lessonTitle" class="theory" v-html="lessonContent"></p>   
      <p v-else class="theory">Jei nori pradėti mokytis, pasirink pamoką iš šoninės juostos.</p>   
    </div>
    <div class="chat">
      <ChatSidePanel />
    </div>
  </div>  
  <Tutorial :isVisible="showTutorial" @close="closeTutorial" />
</template>

<style scoped>

/* .container, .sidebar, .content, .chat {
  border: 1px solid red;
} */

.grid-container {
  display: grid;
  grid-template-areas:
    "sidebar content chat";
  grid-template-columns: 1fr 4fr 1fr;
  gap: 40px;
  height: 100vh;
}

.sidebar {
  grid-area: sidebar;
}

.content {
  grid-area: content;
  display: flex;
  flex-direction: column;
}

.chat {
  grid-area: chat;
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
}

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
}

.theory code {
  font-family: 'Courier New', Courier, monospace;
  color: #f8f8f2;
}
</style>

<style>

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
}

.theory code {
  font-family: 'Courier New', Courier, monospace;
  color: #f8f8f2;
}
.theory a {
  color: #61dafb;
  text-decoration: none;
  font-weight: bold;
}

.theory a:hover {
  text-decoration: underline;
  color: #21a1f1; 
}
.theory table {
  width: 100%;
  border-collapse: collapse;
  margin: 20px 0;
}
.theory th,
.theory td {
  border: 1px solid #ddd;
  padding: 8px;
}
</style>