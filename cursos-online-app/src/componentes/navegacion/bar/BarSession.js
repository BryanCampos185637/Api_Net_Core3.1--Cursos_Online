import { Avatar, Button, IconButton, makeStyles, Toolbar, Typography } from "@material-ui/core";
import React from "react";
import fotoUsuario from '../../../logo.svg'; 

const useStyle = makeStyles((theme) => ({
  seccionDesktop: {
    display: "none",
    [theme.breakpoints.up("md")]: {
      display: "flex",
    },
  },
  seccionMobile: {
    display: "flex",
    [theme.breakpoints.up("md")]: {
      display: "none",
    },
  },
  grow: {
    flexGrow: 1,
  },
  avatarSize:{
      width:40,
      height:40
  }
}));

const BarSession = () => {
    const clases= useStyle();
  return (
    <Toolbar>
      <IconButton color="inherit">
        <i className="material-icons">menu</i>
      </IconButton>

      <Typography variant="h6">Cursos online</Typography>

      <div className={clases.grow}></div>

      <div className={clases.seccionDesktop}>
        <Button color="inherit">Salir</Button>
        <Button color="inherit">{"Nombre de usuario"}</Button>
        <Avatar src={fotoUsuario}></Avatar>
      </div>
      
      <div className={clases.seccionMobile}>
        <IconButton color="inherit">
          <i className="material-icons">more_vert</i>
        </IconButton>
      </div>
    </Toolbar>
  );
};

export default BarSession;
