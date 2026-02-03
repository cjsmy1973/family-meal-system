import axios from 'axios'

const api = axios.create({
  baseURL: '/api',
  timeout: 10000
})

// Ingredients
export const getIngredients = () => api.get('/ingredients')
export const getIngredient = (id: number) => api.get(`/ingredients/${id}`)
export const createIngredient = (data: { name: string; category?: string; unit?: string }) =>
  api.post('/ingredients', data)
export const updateIngredient = (id: number, data: { name: string; category?: string; unit?: string }) =>
  api.put(`/ingredients/${id}`, data)
export const deleteIngredient = (id: number) => api.delete(`/ingredients/${id}`)

// Condiments
export const getCondiments = () => api.get('/condiments')
export const getCondiment = (id: number) => api.get(`/condiments/${id}`)
export const createCondiment = (data: { name: string; unit?: string }) => api.post('/condiments', data)
export const updateCondiment = (id: number, data: { name: string; unit?: string }) =>
  api.put(`/condiments/${id}`, data)
export const deleteCondiment = (id: number) => api.delete(`/condiments/${id}`)

// Recipes
export const getRecipes = () => api.get('/recipes')
export const getRecipe = (id: number) => api.get(`/recipes/${id}`)
export const getRecipeDetail = (id: number) => api.get(`/recipes/${id}/detail`)
export const createRecipe = (data: any) => api.post('/recipes', data)
export const updateRecipe = (id: number, data: any) => api.put(`/recipes/${id}`, data)
export const deleteRecipe = (id: number) => api.delete(`/recipes/${id}`)

// Reservations
export const getReservations = () => api.get('/reservations')
export const getReservation = (id: number) => api.get(`/reservations/${id}`)
export const getShoppingList = (startDate: string, endDate: string) =>
  api.get('/reservations/shopping-list', { params: { startDate, endDate } })
export const createReservation = (data: { reservationDate: string; mealType: string; recipeId: number }) =>
  api.post('/reservations', data)
export const deleteReservation = (id: number) => api.delete(`/reservations/${id}`)

// Upload
export const uploadImage = (file: File) => {
  const formData = new FormData()
  formData.append('file', file)
  return api.post('/upload', formData, {
    headers: { 'Content-Type': 'multipart/form-data' }
  })
}
