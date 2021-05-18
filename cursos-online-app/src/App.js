import React from "react";
import { ThemeProvider as MuiThemeProvider }  from '@material-ui/core/styles';//librerias de material designe
import theme from './theme/theme';//mis objetos de estilo
import Login from "./componentes/seguridad/Login";
import RegistrarUsuario from "./componentes/seguridad/RegistrarUsuario";
import PerfilUsuario from "./componentes/seguridad/PerfilUsuario";
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';// para la navegacion
import { Grid } from "@material-ui/core";
import AppNavbar from "./componentes/navegacion/AppNavbar";



function App() {
  return(
    <Router>
      <MuiThemeProvider theme={theme}>
        <AppNavbar />
        <Grid container>
          <Switch>
            <Route exact path="/auth/login" component={Login}/>
            <Route exact path="/auth/registrar" component={RegistrarUsuario}/>
            <Route exact path="/auth/perfil" component={PerfilUsuario}/>
            <Route exact path="/" component={Login}/>
          </Switch>
        </Grid>
    </MuiThemeProvider>
    </Router>
  );
}

export default App;
