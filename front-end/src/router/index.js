import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import LessonView from '../views/LessonView.vue'
import SolutionEvaluation from '@/components/SolutionEvaluation.vue'
import Cookie from 'js-cookie';
import { isTokenExpired } from '@/api/jwt'
import PurchaseView from '@/views/PurchaseView.vue'
import ProfileView from '@/views/ProfileView.vue'

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
      meta: { guest: true }
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView,
      meta: { guest: true }
    },
    //Konkrečios pamokos maršrutas
    {
      path: '/lessons/:id',
      name: 'lesson',
      component: LessonView,
      props: true,
      meta: { requiresAuth: true }
    },
    //Maršrutas paspaudus "Pamokos" mygtuką navigacijos juostoje
    {
      path: '/lessons',
      name: 'lessons',
      component: LessonView,
      meta: { requiresAuth: true }
    },
    {
      path: '/best-solution',
      name: 'best-solution',
      component: SolutionEvaluation,
      meta: { requiresAuth: true }
    },
    {
      path: '/purchase',
      name: 'purchase',
      component: PurchaseView,
      meta: { requiresAuth: true }
    },
    {
      path: '/profile',
      name: 'profile',
      component: ProfileView,
      meta: { requiresAuth: true }
    },
  ],
})

router.beforeEach((to, from, next) => {
  const token = Cookie.get('AuthToken')

  if (token && isTokenExpired(token)) {
    if (!from.meta.guest && from.fullPath !== '/') {
      localStorage.setItem('lastVisitedRoute', from.fullPath);
    }
    Cookie.remove('AuthToken')
    return next({ name: 'login' })
  }

  const loggedIn = !!token

  if (to.meta.requiresAuth && !loggedIn) {
    if (to.fullPath !== '/') {
      localStorage.setItem('lastVisitedRoute', to.fullPath);
    }
    return next({ name: 'login' })
  }

  if (to.meta.guest && loggedIn) {
    return next({ name: 'home' })
  }

  next()
})

export default router
