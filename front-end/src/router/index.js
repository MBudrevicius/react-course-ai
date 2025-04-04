import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import LessonView from '../views/LessonView.vue'
import SolutionEvaluation from '@/components/SolutionEvaluation.vue'


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
    //Konkrečios pamokos maršrutas
    {
      path: '/lessons/:id',
      name: 'lesson',
      component: LessonView,
      props: true
    },
    //Maršrutas paspaudus "Pamokos" mygtuką navigacijos juostoje
    {
      path: '/lessons',
      name: 'lessons',
      component: LessonView,
    },
    {
      path: '/best-solution',
      name: 'best-solution',
      component: SolutionEvaluation,
    }
  ],
})

export default router
