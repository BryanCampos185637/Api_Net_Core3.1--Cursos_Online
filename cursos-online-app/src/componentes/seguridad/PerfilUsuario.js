import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { obtenerUsuarioActual,actualizarUsuario } from '../../actions/UsuarioAction';
import style from '../Tool/Style';

const PerfilUsuario =()=> {
        const [usuario, setUsuario]=useState({
            nombreCompleto:'',
            email:'',
            password:'',
            confirmarPassword:'',
            userName:''
        });
        
        const ingresarValoresMemory= e=>{
            const {name, value}=e.target;
            setUsuario(anterior => ({
                ...anterior,
                [name]:value
            }))
        }

        useEffect(()=>{
            obtenerUsuarioActual().then(response=>{
                setUsuario(response.data);
            })
        },[]);

        const guardarUsuario=(e)=>{
            e.preventDefault();
            actualizarUsuario(usuario).then(response=>{
                console.log('se actualizo',response);
                window.localStorage.setItem("token_seguridad",response.data.token);
            });
        }

        return (
        <Container component="main" maxWidth="md" justify="center">
            <div style={style.paper}>
                <Typography component="h1" variant="h5">
                    Perfil de usuario
                </Typography>
                <form style={style.form}>
                    <Grid container spacing={2}>
                        <Grid item xs={12} md={12}>
                            <TextField onChange={ingresarValoresMemory} value={usuario.nombreCompleto} name="nombreCompleto" variant="outlined" fullWidth label="Ingrese nombre y apellido" />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField onChange={ingresarValoresMemory} value={usuario.email} type="email" name="email" variant="outlined" fullWidth label="Ingrese email" />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField onChange={ingresarValoresMemory} value={usuario.userName}  name="userName" variant="outlined" fullWidth label="Ingrese username" />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField onChange={ingresarValoresMemory} value={usuario.password} type="password" name="password" variant="outlined" fullWidth label="Ingrese password" />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField onChange={ingresarValoresMemory} value={usuario.confirmarPassword} type="password" name="confirmarPassword" variant="outlined" fullWidth label="Confirme password" />
                        </Grid>
                    </Grid>
                    <Grid container justify="center">
                        <Grid item xs={12} md={6}>
                            <Button type="submit" onClick={guardarUsuario} variant="contained" fullWidth color="primary" size="large" style={style.submit}>
                                Guardar datos
                            </Button>
                        </Grid>
                    </Grid>
                </form>
            </div>
        </Container>        
        );
    }

export default PerfilUsuario;