<template>
  <div class="flex flex-col bg-white dark:bg-zinc-950" style="height: calc(100vh - 64px);">

    <!-- Barra de controles -->
    <div class="px-4 sm:px-6 py-4 border-b border-zinc-100 dark:border-zinc-800 shrink-0">
      <div class="flex flex-col sm:flex-row gap-3 items-stretch sm:items-center">

        <!-- Filtro prioridad -->
        <div class="relative shrink-0">
          <select
            v-model="priorityFilter"
            class="w-full sm:w-44 pl-3 pr-8 py-2.5 rounded-lg border border-zinc-200 dark:border-zinc-800 bg-white dark:bg-zinc-900 text-sm text-zinc-700 dark:text-zinc-200 focus:outline-none focus:ring-2 focus:ring-blue-500/20 shadow-sm appearance-none cursor-pointer"
          >
            <option value="">Todas las prioridades</option>
            <option value="Alta">Alta</option>
            <option value="Media">Media</option>
            <option value="Baja">Baja</option>
          </select>
          <!-- Chevron SVG -->
          <svg class="absolute right-2.5 top-1/2 -translate-y-1/2 text-zinc-400 pointer-events-none" xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="6 9 12 15 18 9"/>
          </svg>
        </div>

        <!-- Buscador -->
        <div class="relative flex-1">
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Buscar tareas..."
            class="w-full pl-9 pr-4 py-2.5 rounded-lg border border-zinc-200 dark:border-zinc-800 bg-white dark:bg-zinc-900 text-sm text-zinc-900 dark:text-zinc-100 placeholder:text-zinc-400 focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-500 transition-all shadow-sm"
          />
          <!-- Search SVG -->
          <svg class="absolute left-3 top-1/2 -translate-y-1/2 text-zinc-400" xmlns="http://www.w3.org/2000/svg" width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/>
          </svg>
        </div>

        <!-- Botón agregar -->
        <button
          @click="openCreate"
          class="flex items-center justify-center gap-2 px-5 py-2.5 rounded-lg bg-blue-600 text-white font-semibold text-sm hover:bg-blue-700 active:bg-blue-800 transition-colors shadow-sm shrink-0"
        >
          <!-- Plus SVG -->
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="12" y1="5" x2="12" y2="19"/><line x1="5" y1="12" x2="19" y2="12"/>
          </svg>
          Agregar
        </button>
      </div>
    </div>

    <!-- Tablero -->
    <div class="flex-1 overflow-auto p-3 sm:p-4">
      <!-- En pantallas grandes: grid de 4 columnas que llenan el espacio -->
      <!-- En pantallas pequeñas: scroll horizontal con columnas de ancho fijo -->
      <div class="hidden lg:grid lg:grid-cols-4 lg:gap-4 h-full">
        <div
          v-for="status in COLUMNS"
          :key="status"
          class="min-w-0 h-full"
        >
          <TaskColumn
            :status="status"
            :tasks="tasksByStatus(status)"
            @taskDrop="handleTaskDrop"
            @edit="openEdit"
            @delete="handleDelete"
          />
        </div>
      </div>

      <!-- Mobile / tablet: scroll horizontal -->
      <div class="flex lg:hidden gap-3 h-full" style="min-width: max-content; width: 100%;">
        <div
          v-for="status in COLUMNS"
          :key="status"
          class="w-72 h-full shrink-0"
        >
          <TaskColumn
            :status="status"
            :tasks="tasksByStatus(status)"
            @taskDrop="handleTaskDrop"
            @edit="openEdit"
            @delete="handleDelete"
          />
        </div>
      </div>
    </div>

    <!-- Loading overlay -->
    <Transition name="fade">
      <div v-if="globalLoading" class="fixed inset-0 bg-black/20 z-40 flex items-center justify-center">
        <div class="bg-white dark:bg-zinc-900 rounded-xl px-6 py-4 shadow-xl flex items-center gap-3 text-zinc-700 dark:text-zinc-200 text-sm font-medium">
          <svg class="animate-spin text-blue-600" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M21 12a9 9 0 1 1-6.219-8.56"/>
          </svg>
          Cargando tareas...
        </div>
      </div>
    </Transition>

    <!-- Toast de notificación -->
    <Transition name="toast">
      <div
        v-if="toast.show"
        :class="[
          'fixed bottom-6 right-6 z-50 px-4 py-3 rounded-xl shadow-lg text-sm font-medium flex items-center gap-2 max-w-xs',
          toast.type === 'success' ? 'bg-emerald-600 text-white' : 'bg-red-600 text-white'
        ]"
      >
        <svg v-if="toast.type === 'success'" xmlns="http://www.w3.org/2000/svg" width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
          <polyline points="20 6 9 17 4 12"/>
        </svg>
        <svg v-else xmlns="http://www.w3.org/2000/svg" width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/>
        </svg>
        {{ toast.message }}
      </div>
    </Transition>

    <!-- Modal -->
    <TaskModal
      :is-open="isModalOpen"
      :mode="modalMode"
      :initial-data="selectedTask"
      :loading="modalLoading"
      @close="isModalOpen = false"
      @submit="handleModalSubmit"
    />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import TaskColumn from './TaskColumn.vue'
import TaskModal from './TaskModal.vue'
import { taskService } from '../services/api.js'

const COLUMNS = ['Pendiente', 'En proceso', 'Revisión', 'Finalizado']

const tasks = ref([])
const searchQuery = ref('')
const priorityFilter = ref('')
const globalLoading = ref(false)
const isModalOpen = ref(false)
const modalMode = ref('create')
const selectedTask = ref(null)
const modalLoading = ref(false)

// Toast
const toast = ref({ show: false, message: '', type: 'success' })
let toastTimer = null

function showToast(message, type = 'success') {
  if (toastTimer) clearTimeout(toastTimer)
  toast.value = { show: true, message, type }
  toastTimer = setTimeout(() => { toast.value.show = false }, 3000)
}

// Tareas filtradas
const filteredTasks = computed(() => {
  return tasks.value.filter(t => {
    const matchSearch = t.title.toLowerCase().includes(searchQuery.value.toLowerCase())
    const matchPriority = !priorityFilter.value || t.priority === priorityFilter.value
    return matchSearch && matchPriority
  })
})

function tasksByStatus(status) {
  return filteredTasks.value.filter(t => t.status === status)
}

// Cargar tareas
async function loadTasks() {
  globalLoading.value = true
  try {
    const response = await taskService.getAll()
    const data = response.data
    tasks.value = Array.isArray(data) ? data : (data.$values ?? data.value ?? [])
  } catch (e) {
    console.error('Error al cargar tareas', e)
    tasks.value = []
    showToast('Error al cargar las tareas', 'error')
  } finally {
    globalLoading.value = false
  }
}

function openCreate() {
  modalMode.value = 'create'
  selectedTask.value = null
  isModalOpen.value = true
}

function openEdit(task) {
  modalMode.value = 'edit'
  selectedTask.value = { ...task }
  isModalOpen.value = true
}

async function handleModalSubmit(formData) {
  modalLoading.value = true
  try {
    if (modalMode.value === 'create') {
      const { data } = await taskService.create(formData)
      tasks.value.unshift(data)
      showToast('Tarea creada correctamente')
    } else {
      const { data } = await taskService.update(selectedTask.value.id, formData)
      const idx = tasks.value.findIndex(t => t.id === selectedTask.value.id)
      if (idx !== -1) tasks.value[idx] = data
      showToast('Tarea actualizada')
    }
    isModalOpen.value = false
  } catch (e) {
    console.error('Error al guardar', e)
    showToast('Error al guardar la tarea', 'error')
  } finally {
    modalLoading.value = false
  }
}

async function handleDelete(taskId) {
  if (!confirm('¿Eliminar esta tarea?')) return
  try {
    await taskService.delete(taskId)
    tasks.value = tasks.value.filter(t => t.id !== taskId)
    showToast('Tarea eliminada')
  } catch (e) {
    console.error('Error al eliminar', e)
    showToast('Error al eliminar', 'error')
  }
}

async function handleTaskDrop({ taskId, newStatus }) {
  const task = tasks.value.find(t => String(t.id) === String(taskId))
  if (!task) return

  const prevStatus = task.status
  task.status = newStatus // Actualización optimista

  try {
    await taskService.updateStatus(task.id, newStatus)
    showToast(`Tarea movida a "${newStatus}"`)
  } catch (e) {
    task.status = prevStatus
    showToast('Error al mover la tarea', 'error')
    console.error('Error al cambiar estado', e)
  }
}

onMounted(loadTasks)
</script>

<style scoped>
.fade-enter-active, .fade-leave-active { transition: opacity 0.2s; }
.fade-enter-from, .fade-leave-to { opacity: 0; }

.toast-enter-active, .toast-leave-active { transition: all 0.3s ease; }
.toast-enter-from { opacity: 0; transform: translateY(12px) scale(0.95); }
.toast-leave-to { opacity: 0; transform: translateY(12px) scale(0.95); }
</style>