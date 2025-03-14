import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import LessonView from '../views/LessonView.vue' // Importuokite pamokos komponentą


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView,
    },
    {
      path: '/lessons/:id',
      name: 'lesson',
      component: LessonView,
      props: true
    },
    {
      path: '/lessons',
      name: 'lessons',
      component: LessonView,
    },
  ],
})

export default router
