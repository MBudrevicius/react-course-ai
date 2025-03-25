<script setup>
import Navbar from '../components/Navbar.vue'
import SideBar from '../components/Sidebar.vue'
import Task from '@/components/Task.vue';
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { getLessonById, getTasksByLessonId } from '../api/lessonAPI'
import UploadFile from '@/components/UploadFile.vue';
import ChatSidePanel from '@/components/ChatSidePanel.vue';

const route = useRoute()
const lessonId = ref(route.params.id)
const lessonTitle = ref('')
const lessonContent = ref('')
const taskContent = ref('No tasks available')

onMounted(async () => {
  fetchLesson();
  fetchTasks();
})

watch(() => route.params.id, async (newId) => {
  lessonId.value = newId;
  fetchLesson();
  fetchTasks();
});

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
    <div class="container">
      <div class="sidebar">
        <SideBar />
      </div>
      <div class="content">
        <Task :taskContent="taskContent" />
        <h1 v-if="!lessonTitle" class="theory">Labas!</h1>
        <h1 v-else class="theory">{{ lessonTitle }}</h1>
        <p v-if="!lessonTitle" class="theory">Jei nori pradėti mokytis, pasirink pamoką iš šoninės juostos.</p>
        <p v-else class="theory">{{ lessonContent }}</p>
        <UploadFile v-if="lessonTitle"/>
      </div>
    </div>
    <ChatSidePanel />

</template>

<style scoped>
.container {
  display: grid;
  grid-template-areas:
    "sidebar content";
  grid-template-columns: 1fr 3fr;
}

.sidebar {
  grid-area: sidebar;
}

.content {
  grid-area: content;
  display: flex;
  flex-direction: column;
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
    font-size: 32px;
    text-align: justify;
}
</style>