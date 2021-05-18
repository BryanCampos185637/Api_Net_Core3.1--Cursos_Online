import { Button, Container, Grid, TextField, Typography } from "@material-ui/core";
import React, { useState } from "react";
import style from '../Tool/Style';
import {resgistrarUsuario} from '../../actions/UsuarioAction'

const RegistrarUsuario = () => 
{
    //mi json que se enviara a la api
    const [usuario, setUsuario]=useState({
        NombreCompleto:'',
        Email:'',
        Password:'',
        ConfirmarPassword:'',
        UserName:''
    });

    const ingresarValoresMemoria= e=>{
        const {name, value}= e.target;
        setUsuario(anterior => ({
            ...anterior,
            [name]:value
        }))
    };

    const RegistrarUsuarioBoton=e=>{
        e.preventDefault();
        resgistrarUsuario(usuario).then(response=>{
            console.log('se registro el usuario',response);
            window.localStorage.setItem("token_seguridad", response.data.token);
            alert('se registro el usuario');
        });
    };

  return(
    <Container component="main" maxWidth="md" justify="center">
        <div style={style.paper}>
            <Typography component="h1" variant="h5">
                Registro de usuarios
            </Typography>
            <form style={style.form}>
                <Grid container spacing={2}>
                    <Grid item xs={12} md={12}>
                        <TextField name="NombreCompleto" variant="outlined" value={usuario.NombreCompleto} onChange={ingresarValoresMemoria} fullWidth label="Ingrese nombre y apellidos"></TextField>
                    </Grid>    
                    <Grid item xs={12} md={6}>
                        <TextField name="Email" type="email" variant="outlined" value={usuario.Email} onChange={ingresarValoresMemoria} fullWidth label="Ingrese su email"></TextField>
                    </Grid>
                    <Grid  item xs={12} md={6}>
                        <TextField name="UserName" variant="outlined" value={usuario.UserName} onChange={ingresarValoresMemoria} fullWidth label="Ingrese su nombre usuario"></TextField>
                    </Grid>
                    <Grid  item xs={12} md={6}>
                        <TextField name="Password" type="password" variant="outlined" value={usuario.Password} onChange={ingresarValoresMemoria} fullWidth label="Ingrese su contraseña"></TextField>
                    </Grid>
                    <Grid  item xs={12} md={6}>
                        <TextField name="ConfirmarPassword" type="password" variant="outlined" value={usuario.ConfirmarPassword} onChange={ingresarValoresMemoria} fullWidth label="Confirme su contraseña"></TextField>
                    </Grid>
                </Grid>    
                <Grid container justify="center">
                    <Grid item xs={12} md={6}>
                        <Button type="submit" fullWidth variant="contained" color="primary" size="large" style={style.submit} onClick={RegistrarUsuarioBoton}>
                            Guardar
                        </Button>
                    </Grid>    
                </Grid>            
            </form>
        </div>
    </Container>
    );
};
export default RegistrarUsuario;
