<template>
  <div :class="['min-h-screen transition-colors duration-200', darkMode ? 'dark' : '']">
    <!-- Navbar -->
    <nav class="sticky top-0 z-30 w-full h-16 bg-white dark:bg-zinc-900 border-b border-zinc-200 dark:border-zinc-800 flex items-center justify-between px-4 sm:px-8 shadow-sm">
      
      <!-- Logo / Título -->
      <div class="flex items-center gap-2.5">
        <div class="w-8 h-8 rounded-lg bg-blue-600 flex items-center justify-center shrink-0">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <rect x="3" y="3" width="7" height="9"/><rect x="14" y="3" width="7" height="5"/>
            <rect x="14" y="12" width="7" height="9"/><rect x="3" y="16" width="7" height="5"/>
          </svg>
        </div>
        <span class="text-base font-bold text-zinc-900 dark:text-zinc-100 tracking-tight">Task Manager</span>
      </div>

      <!-- Controles derecha -->
      <div class="flex items-center gap-2">
        <!-- Toggle dark mode -->
        <button
          @click="darkMode = !darkMode"
          class="p-2 rounded-full hover:bg-zinc-100 dark:hover:bg-zinc-800 text-zinc-500 dark:text-zinc-400 transition-colors"
          :title="darkMode ? 'Modo claro' : 'Modo oscuro'"
        >
          <!-- Sun SVG -->
          <svg v-if="darkMode" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="5"/>
            <line x1="12" y1="1" x2="12" y2="3"/><line x1="12" y1="21" x2="12" y2="23"/>
            <line x1="4.22" y1="4.22" x2="5.64" y2="5.64"/><line x1="18.36" y1="18.36" x2="19.78" y2="19.78"/>
            <line x1="1" y1="12" x2="3" y2="12"/><line x1="21" y1="12" x2="23" y2="12"/>
            <line x1="4.22" y1="19.78" x2="5.64" y2="18.36"/><line x1="18.36" y1="5.64" x2="19.78" y2="4.22"/>
          </svg>
          <!-- Moon SVG -->
          <svg v-else xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"/>
          </svg>
        </button>

        <div class="w-px h-5 bg-zinc-200 dark:bg-zinc-700 mx-1" />

        <!-- Avatar -->
        <div class="flex items-center gap-2.5">
          <div class="hidden sm:flex flex-col items-end">
            <span class="text-sm font-semibold text-zinc-900 dark:text-zinc-200 leading-tight">Admin</span>
            <span class="text-xs text-zinc-400 leading-tight">Administrador</span>
          </div>
          <div class="w-8 h-8 rounded-full bg-blue-100 dark:bg-blue-900/30 border border-blue-200 dark:border-blue-800 flex items-center justify-center">
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#2563eb" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
              <circle cx="12" cy="7" r="4"/>
            </svg>
          </div>
        </div>
      </div>
    </nav>

    <!-- Contenido principal -->
    <KanbanBoard />
  </div>
</template>

<script setup>
import { ref } from 'vue'
import KanbanBoard from './components/KanbanBoard.vue'

const darkMode = ref(false)
</script>