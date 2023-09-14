import axios from "axios";
import { useQuery } from "react-query";
import { UserInfo } from "../types/userInfo";

const fetchUserInfo = async (username?: string): Promise<UserInfo> => {
  const { data } = await axios.get("/api/userinfo", { params: { username } });
  console.log("fetchUserInfo username" + data.email)
  return data;
};

export function useUserInfo(username?: string) {
  return useQuery(["user-info", username], () => fetchUserInfo(username), {
    enabled: !!username,
  });
}
