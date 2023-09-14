import axios from "axios";
import { useQuery } from "react-query";
import { ProfileInfo } from "../types/profileInfo";

const fetchProfileInfo = async (): Promise<ProfileInfo> => {
  const { data } = await axios.get("/api/profileinfo");
  return data;
};

export function useProfileInfo() {
  return useQuery("profile-info", () => fetchProfileInfo());
}
