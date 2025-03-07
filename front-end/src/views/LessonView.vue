<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { getLessonById } from '../api/lessonAPI'

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
  <div>
    <p>Pamokos pavadinimas: {{ lessonTitle }}</p>
    <p>Pamokos turinys: {{ lessonContent }}</p>
  </div>
</template>