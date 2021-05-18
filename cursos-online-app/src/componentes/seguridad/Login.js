import { Avatar, Button, Container, TextField, Typography } from '@material-ui/core';
import React, { useState } from 'react';
import style from '../Tool/Style';
import LockOutlined from '@material-ui/icons/LockOutlined';
import { loginUsuario } from '../../actions/UsuarioAction';

const Login = () => {
    const [usuario,setUsuario]=useState({
        Email:'',
        Password:'',
    });
    const ingresarValoresMemoria=(e)=>{
        const {name,value}= e.target;
        setUsuario(anterior=>({
                ...anterior,
                [name]:value
        }));
    }
    const logInUsuario=(e)=>{
        e.preventDefault();
        loginUsuario(usuario).then((response) => {
            console.log("Se inicio sesión", response);
            window.localStorage.setItem('token_seguridad',response.data.token);
            alert('Bienvenido '+response.data.nombreCompleto);
        });
    }
    return (
        <Container maxWidth="xs" >
            <div style={style.paper}>
                <Avatar style={style.avatar}>
                    <LockOutlined style={style.icon}/>
                </Avatar>
                <Typography component="h1" variant="h5">
                    Login de usuario
                </Typography>
                <form style={style.form}>
                    <TextField variant="outlined" onChange={ingresarValoresMemoria} value={usuario.Email} type="email" label="Ingrese su email" name="Email" fullWidth margin="normal"></TextField>
                    <TextField variant="outlined" onChange={ingresarValoresMemoria} value={usuario.Password} type="password" label="Ingrese su password" name="Password" fullWidth margin="normal"></TextField>
                    <Button type="submit" onClick={logInUsuario} fullWidth variant="contained" color="primary" style={style.submit}>
                        Entrar
                    </Button>
                </form>
            </div>
        </Container>                
    );
};

export default Login;