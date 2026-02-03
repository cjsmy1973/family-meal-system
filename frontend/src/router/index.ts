import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: () => import('@/views/Home.vue')
  },
  {
    path: '/ingredients',
    name: 'Ingredients',
    component: () => import('@/views/Ingredients.vue')
  },
  {
    path: '/condiments',
    name: 'Condiments',
    component: () => import('@/views/Condiments.vue')
  },
  {
    path: '/recipes',
    name: 'Recipes',
    component: () => import('@/views/Recipes.vue')
  },
  {
    path: '/recipes/create',
    name: 'CreateRecipe',
    component: () => import('@/views/RecipeForm.vue')
  },
  {
    path: '/recipes/:id/edit',
    name: 'EditRecipe',
    component: () => import('@/views/RecipeForm.vue')
  },
  {
    path: '/reservations',
    name: 'Reservations',
    component: () => import('@/views/Reservations.vue')
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
