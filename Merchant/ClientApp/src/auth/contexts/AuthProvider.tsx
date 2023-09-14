import React, { createContext, useContext } from "react";
import { useLogin } from "../hooks/useLogin";
import { useLogout } from "../hooks/useLogout";
import { useUserInfo } from "../hooks/useUserInfo";
import { UserInfo } from "../types/userInfo";
import {AuthInfo} from "../../api/types/authInfo";
import { useAuthInfoStore } from "../../api/types/authInfo";

interface AuthContextInterface {
  hasRole: (roles?: string[]) => {};
  isLoggingIn: boolean;
  isLoggingOut: boolean;
  login: (email: string, password: string) => Promise<any>;
  logout: () => Promise<any>;
  userInfo?: UserInfo;
}

export const AuthContext = createContext({} as AuthContextInterface);

type AuthProviderProps = {
  children?: React.ReactNode;
};

const AuthProvider = ({ children }: AuthProviderProps) => {
  const { isLoggingIn, login } = useLogin();
  const { isLoggingOut, logout } = useLogout();
  // @ts-ignore
    const { data: userInfo } = useUserInfo(useAuthInfoStore.getState().userName);

  const hasRole = (roles?: string[]) => {
    if (!roles || roles.length === 0) {
      return true;
    }
    if (!userInfo) {
      return false;
    }
    return roles.includes(userInfo.role);
  };

  const handleLogin = async (username: string, password: string) => {
    return login({ username, password })
      .then((key: AuthInfo) => {
          useAuthInfoStore.setState( state => { // @ts-ignore
              state.setAuth(key)})
          console.log("Key "+ key.accessToken);
          // @ts-ignore
          console.log("Zustand store "+ useAuthInfoStore.getState().accessToken);

        return key;
      })
      .catch((err: any) => {
          console.log("Error " + err)
        throw err;
      });
  };

  const handleLogout = async () => {
    return logout()
      .then((data: any) => {
          useAuthInfoStore.setState( state => { // @ts-ignore
              state.setAuth({})})
        return data;
      })
      .catch((err: any) => {
        throw err;
      });
  };

  return (
    <AuthContext.Provider
      value={{
        hasRole,
        isLoggingIn,
        isLoggingOut,
        login: handleLogin,
        logout: handleLogout,
        userInfo,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export function useAuth() {
  return useContext(AuthContext);
}

export default AuthProvider;
