import {
  Avatar,
  Box,
  Button,
  Card,
  CardContent,
  IconButton,
  Typography
} from "@mui/material";
import AddIcon from '@mui/icons-material/Add';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import React,{ useEffect, useState} from "react";
import { useTranslation } from "react-i18next";
import { Link as RouterLink } from "react-router-dom";
import axios from "axios";
import { Merchant } from "../types/merchant";
import { useAuthInfoStore } from "../../api/types/authInfo";


const MerchantWidgets = () => {
  const { t } = useTranslation();

  const [merchant, setMerchant] = useState<Merchant[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () =>{
      setLoading(true);
      try 
      {
        // @ts-ignore
        console.log("at login data Access Token  " + useAuthInfoStore.getState().accessToken);
        const {data } = await axios.get<Merchant[]>('http://localhost:5555/paymentgateway/payment', {
         
          headers: {
             // @ts-ignore
            'Authorization': `bearer ${useAuthInfoStore.getState().accessToken}`
          }
        });

        setMerchant(data);
      } catch (error:any) {
        console.error(error.message);
      }
      setLoading(false);
    }

    fetchData();

   
  }, []);

  return (
    <React.Fragment>
      <Typography component="h2" marginBottom={3} variant="h4">
        {t("admin.home.merchant.title")}
      </Typography>
      {merchant.map((mech : Merchant) => (
        <Card key={mech.id} sx={{ mb: 2 }}>
          <CardContent sx={{ display: "flex", alignItems: "center" }}>
            <Avatar
              alt={`${mech.person} avatar`}
              src={mech.image}
              sx={{ mr: 2 }}
            />
            <Box sx={{ flexGrow: 1 }}>
              <Typography component="div" variant="h6">
                {mech.person}
              </Typography>
              <Typography variant="body2" color="textSecondary" component="div">
                {mech.date}
              </Typography>
            </Box>
            <IconButton
              aria-label="Go to merchant details"
              component={RouterLink}
              to={`/admin/calendar`}
            >
              <ChevronRightIcon />
            </IconButton>
          </CardContent>
        </Card>
      ))}
      <Button
        aria-label="Add new merchant"
        color="secondary"
        component={RouterLink}
        fullWidth
        to={`/admin/calendar`}
        variant="contained"
      >
        <AddIcon />
      </Button>
    </React.Fragment>
  );
};

export default MerchantWidgets;
