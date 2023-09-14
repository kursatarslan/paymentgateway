import create from "zustand";

export type AuthInfo = 
{
    status: string;
    accessToken: string;
    refreshToken: string;
    userName: string;
}

export type AuthInfoReducers = {
    setAccessToken: (val: string) => void;
    getAccessToken: () => number;
    setAuth: (state: AuthInfo) => void;
    getAuth: () => AuthInfo;
};

export type AuthInfoStore = AuthInfo & AuthInfoReducers;

// @ts-ignore
export const useAuthInfoStore = create<AuthInfoStore>((set, get) => ({
    status: null,
    accessToken: "",
    refreshToken: "",
    userName: null,
    setAccessToken: (val: string) => {
        set({ accessToken: val });
    },
    getAccessToken: () => {
        return get().accessToken;
    },
    setAuth: (state: AuthInfo) => {
        set({ ...state });
    },

    getAuth: () => {
        return {
            AccessToken: get().accessToken,
            status: get().status,
            RefreshToken: get().refreshToken,
            UserName: get().userName,
        };
    },
}));