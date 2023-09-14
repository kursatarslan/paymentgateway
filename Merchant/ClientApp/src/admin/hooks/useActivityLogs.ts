import axios from "axios";
import { useQuery } from "react-query";
import { ActivityLog } from "../types/activityLog";

const fetchActivityLogs = async (): Promise<ActivityLog[]> => {
  const { data } = await axios.get("/api/activitylogs");
  return data;
};

export function useActivityLogs() {
  return useQuery("activity-logs", () => fetchActivityLogs());
}
