<template>
  <div id="app">
    <header>
      <h1>Control de Temperaturas</h1>
      <nav>
        <router-link to="/">Listado de formatos</router-link> |
        <router-link to="/nuevo">Nuevo Formato</router-link> |
        <router-link to="/reporte">Reporte</router-link>
      </nav>
    </header>

    <main>
      <router-view></router-view>
    </main>
  </div>
</template>

<script>
export default {};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  margin: 20px;
}

nav a {
  margin-right: 10px;
  text-decoration: none;
  color: #42b983;
}

nav a.router-link-active {
  font-weight: bold;
}
</style>
