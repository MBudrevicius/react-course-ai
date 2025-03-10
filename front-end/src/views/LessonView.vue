<script setup>
import Navbar from '../components/Navbar.vue'
import SideBar from '../components/Sidebar.vue'
import Task from '@/components/Task.vue';
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { getLessonById } from '../api/lessonAPI'
import UploadFile from '@/components/UploadFile.vue';

const route = useRoute()
const lessonId = ref(route.params.id)
const lessonTitle = ref('')
const lessonContent = ref('')

onMounted(async () => {
  try {
    const lesson = await getLessonById(lessonId.value)
    lessonTitle.value = lesson.title
    lessonContent.value = lesson.content
    console.log('Pamokos pavadinimas:', lessonTitle.value)  
    console.log('Pamokos turinys:', lessonContent.value)
  } catch (error) {
    console.log('Error fetching lesson:', error)
  }
})
</script>

<template>
    <Navbar />
    <div class="container">
      <div class="sidebar">
        <SideBar />
      </div>
      <div class="content">
        <Task />
        <h1 v-if="!lessonTitle" class="theory">Labas!</h1>
        <h1 v-else class="theory">{{ lessonTitle }}</h1>
        <p v-if="!lessonTitle" class="theory">Jei nori pradėti mokytis, pasirink pamoką iš šoninės juostos.</p>
        <p v-else class="theory">{{ lessonContent }}</p>
        <UploadFile v-if="lessonTitle"/>
      </div>
    </div>
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