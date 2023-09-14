import React, {useEffect, useState} from 'react';
import { toast } from 'react-toastify';
import "react-toastify/dist/ReactToastify.css";
import {
    Box,
    Card,
    CardContent,
    CardHeader,
    LinearProgress,
    List,
    ListItem,
    ListItemText,
    Typography,
    Button,
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import axios from 'axios';
import { useAuthInfoStore } from '../../api/types/authInfo';
import { Coin } from '../types/coin';
import PaymentForm from '../components/PaymentForm';
import { PaymentFormValues } from "../types/PaymentFromValues";

const PaymentsWidget = () => {
    const { t } = useTranslation();
    const [loading, setLoading] = useState(true);
    const [isSubmitting, setIsSubmitting] = useState(false); // Initialize the loading state
    // Initialize the payment form data
    const initialPaymentData: PaymentFormValues = {
        firstName: '',
        lastName: '',
        email: '',
        creditCardNumber: '',
        amount: 0, // Initialize the amount field
        // Initialize other fields as needed
    };

    const [paymentData, setPaymentData] = useState<PaymentFormValues>(
        initialPaymentData
    );
    const submitPaymentForm = async (formData: PaymentFormValues) => {
        try {
            setIsSubmitting(true); // Set loading state to true when submitting
            console.log('formData', formData.email);
            const response =  await axios.post(
                process.env.REACT_APP_APIGATEWAY_URL + '/paymentgateway/payment',
                formData,
                {
                    headers: {
                        // @ts-ignore
                        Authorization: `bearer ${useAuthInfoStore.getState().accessToken}`,
                    },
                }
            );
            // Handle success or any other logic after submitting
            console.log('Form data submitted successfully:', formData);
            const responseData = response.data;
            console.log('Response data:', responseData);
            if (responseData.success === false) {
                // Show a warning using react-toastify
                toast.warning(responseData.errorMessage);
                console.log('responseData.success === false:');
            }else {
                console.log('responseData.success === true:');
                toast.success("Payment successfully sent ");
            }
            
        } catch (error) {
            // Handle errors if the submission fails
            console.error('Error submitting form data:' + error);
        } finally {
            setIsSubmitting(false); // Set loading state back to false when submission is complete
        }
    };

    return (
        <div>
            <h2>{t('paymentWidgets.title')}</h2>
            {/* Display the PaymentForm */}
            <PaymentForm
                formData={paymentData}
                onSubmit={submitPaymentForm}
                isSubmitting={isSubmitting}
            />
            {isSubmitting && <LinearProgress />} {/* Display a loading indicator */}
        </div>
    );
};

export default PaymentsWidget;
