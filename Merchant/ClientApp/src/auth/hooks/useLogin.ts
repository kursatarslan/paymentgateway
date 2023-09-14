import { useMutation } from "react-query";
import {AuthInfo} from "../../api/types/authInfo";
import axios from "axios";


const login = async ({username, password}: {
    username: string;
    password: string;
  }): Promise<AuthInfo> => {
    const { data } = await axios.post("/api/auth/login", { username, password });
    console.log("at login data Access Token  " + data.accessToken)
    return data;
  };

export function useLogin() {
  const { isLoading, mutateAsync } = useMutation(login);

  return { isLoggingIn: isLoading, login: mutateAsync };
}
