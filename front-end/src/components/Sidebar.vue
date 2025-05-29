<script setup>
import { ref, onMounted } from 'vue';
import { getLessonsTitles } from '@/api/lessonAPI';
import { getCookie } from '@/api/APIRequest';

const lessons = ref([]);
const isPremiumUser = ref(false);
const emit = defineEmits(['premiumRequired']);

onMounted(async () => {
  isPremiumUser.value = getCookie('UserType') === 'premium';
  try {
    const data = await getLessonsTitles();
    console.log("111:" + data);
    lessons.value = data || [];
  } catch (error) {}
})

</script>

<template>
  <div class="sidebar-container">
    <div class="sidebar">
      <div class="sidebar-item">
        <img src="/svg/book.svg" class="svg" />
      </div>
      <ul>
        <li v-for="(lesson, index) in lessons" :key="lesson.id">
          <router-link
            v-if="!lesson.premium || isPremiumUser"
            :to="`/lessons/${lesson.id}`"
            class="lesson-link"
          >
            {{ index + 1 }}. {{ lesson.title }}
          </router-link>
          <a
            v-else
            class="lesson-link"
            :class="{ disabled: lesson.premium && !isPremiumUser }"
            @click = "emit('premiumRequired')"
          >
            {{ index + 1 }}. {{ lesson.title }}
        </a>
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
  box-shadow: 3px 0 10px rgba(0, 0, 0, 0.4) inset;
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

.lesson-link.disabled {
  color: grey;
  cursor: pointer;
}

</style>