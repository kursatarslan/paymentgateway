import React, { useState } from 'react';
import {
    Box,
    Grid,
    Paper,
    TextField,
    Typography,
} from '@mui/material';
import LoadingButton from '@mui/lab/LoadingButton';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { useSnackbar } from '../../core/contexts/SnackbarProvider';

import BoxedLayout from '../../core/components/BoxedLayout';

// Import the PaymentFormValues interface
import { PaymentFormValues } from '../types/PaymentFromValues';

interface PaymentFormProps {
    onSubmit: (formData: PaymentFormValues) => Promise<void>;
    isSubmitting: boolean;
    formData: PaymentFormValues; // Add the formData prop here
}

const PaymentForm: React.FC<PaymentFormProps> = ({ onSubmit, isSubmitting,formData }) => {
    const snackbar = useSnackbar();

    const formik = useFormik({
        initialValues: formData,
        validationSchema: Yup.object({
            firstName: Yup.string().required('First Name is required'),
            lastName: Yup.string().required('Last Name is required'),
            email: Yup.string()
                .email('Invalid email address')
                .required('Email is required'),
            creditCardNumber: Yup.string().required('Credit Card Number is required'),
            amount: Yup.number()
                .typeError('Amount must be a number')
                .positive('Amount must be positive')
                .required('Amount is required'),
        }),
        onSubmit: (values) => {
            // Send the form data to the parent component for submission
            onSubmit(values);
        },
    });

    return (
        <Grid
            container
            component="main"
            sx={{ height: '100vh' }}
        >
            <Grid
                item
                xs={24}
                sm={16}
                md={10}
                component={Paper}
                square
            >
                <BoxedLayout>
                    <Typography component="h1" variant="h5">
                        Payment Form
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
                            id="firstName"
                            label="First Name"
                            name="firstName"
                            autoFocus
                            disabled={isSubmitting}
                            value={formik.values.firstName}
                            onChange={formik.handleChange}
                            error={formik.touched.firstName && Boolean(formik.errors.firstName)}
                            helperText={formik.touched.firstName && formik.errors.firstName}
                        />
                        <TextField
                            margin="normal"
                            variant="filled"
                            required
                            fullWidth
                            name="lastName"
                            label="Last Name"
                            autoFocus
                            disabled={isSubmitting}
                            value={formik.values.lastName}
                            onChange={formik.handleChange}
                            error={formik.touched.lastName && Boolean(formik.errors.lastName)}
                            helperText={formik.touched.lastName && formik.errors.lastName}
                        />
                        <TextField
                            margin="normal"
                            variant="filled"
                            required
                            fullWidth
                            name="email"
                            label="Email"
                            autoComplete="email"
                            disabled={isSubmitting}
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
                            name="creditCardNumber"
                            label="Credit Card Number"
                            type="password"
                            autoComplete="current-password"
                            disabled={isSubmitting}
                            value={formik.values.creditCardNumber}
                            onChange={formik.handleChange}
                            error={formik.touched.creditCardNumber && Boolean(formik.errors.creditCardNumber)}
                            helperText={formik.touched.creditCardNumber && formik.errors.creditCardNumber}
                        />
                        <TextField
                            margin="normal"
                            variant="filled"
                            required
                            fullWidth
                            name="amount"
                            label="Amount"
                            type="number"
                            disabled={isSubmitting}
                            value={formik.values.amount}
                            onChange={formik.handleChange}
                            error={formik.touched.amount && Boolean(formik.errors.amount)}
                            helperText={formik.touched.amount && formik.errors.amount}
                        />
                        <LoadingButton
                            type="submit"
                            fullWidth
                            loading={isSubmitting}
                            variant="contained"
                            sx={{ mt: 3 }}
                        >
                            Submit Payment
                        </LoadingButton>
                    </Box>
                </BoxedLayout>
            </Grid>
        </Grid>
    );
};

export default PaymentForm;
