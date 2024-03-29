import { Box, Grid, Paper, TextField, Typography } from "@mui/material";
import LoadingButton from "@mui/lab/LoadingButton";
import { useFormik } from 'formik';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import * as Yup from 'yup';
import BoxedLayout from "../../core/components/BoxedLayout";
import { useSnackbar } from "../../core/contexts/SnackbarProvider";
import { useAuth } from "../contexts/AuthProvider";

const Login = () => {
  const { isLoggingIn, login } = useAuth();
  const navigate = useNavigate();
  const snackbar = useSnackbar();
  const { t } = useTranslation();

  const handleLogin = (email: string, password: string) => {
    login(email, password)
      .then(() => navigate(`/greenfield`, { replace: true }))
      .catch((err) =>  {
        console.log("data Access Token " + err)
        snackbar.error(t("common.errors.unexpected.subTitle"))
      });
  };

  const formik = useFormik({
    initialValues: {
      email: "",
      password: ""
    },
    validationSchema: Yup.object({
      email: Yup.string()
        .email(t("common.validations.email"))
        .required(t("common.validations.required")),
      password: Yup.string()
        .min(8, t("common.validations.min", { size: 8 }))
        .required(t("common.validations.required"))
    }),
    onSubmit: (values: { email: string; password: string; }) => {
      return handleLogin(values.email, values.password);
    }
  });

  return (
    <Grid container component="main" sx={{ height: "100vh" }}>
      <Grid
        item
        xs={false}
        sm={4}
        md={7}
        sx={{
          backgroundImage: "url(./img/startup.svg)",
          backgroundRepeat: "no-repeat",
          bgcolor: "background.default",
          backgroundSize: "cover",
          backgroundPosition: "center"
        }}
      />
      <Grid item xs={12} sm={8} md={5} component={Paper} square>
        <BoxedLayout>
          <Typography component="h1" variant="h5">
            {t("auth.login.title")}
          </Typography>
          <Box
            component="form"
            marginTop={3}
            noValidate
            onSubmit={formik.handleSubmit}
          >
            <TextField
              margin="normal"
              variant="filled"
              required
              fullWidth
              id="email"
              label={t("auth.login.form.email.label")}
              name="email"
              autoComplete="email"
              autoFocus
              disabled={isLoggingIn}
              value={formik.values.email}
              onChange={formik.handleChange}
              error={formik.touched.email && Boolean(formik.errors.email)}
              helperText={formik.touched.email && formik.errors.email}
            />
            <TextField
              margin="normal"
              variant="filled"
              required
              fullWidth
              name="password"
              label={t("auth.login.form.password.label")}
              type="password"
              id="password"
              autoComplete="current-password"
              disabled={isLoggingIn}
              value={formik.values.password}
              onChange={formik.handleChange}
              error={formik.touched.password && Boolean(formik.errors.password)}
              helperText={formik.touched.password && formik.errors.password}
            />
            <LoadingButton
              type="submit"
              fullWidth
              loading={isLoggingIn}
              variant="contained"
              sx={{ mt: 3 }}
            >
              {t("auth.login.submit")}
            </LoadingButton>
          </Box>
        </BoxedLayout>
      </Grid>
    </Grid>
  );
};

export default Login;
