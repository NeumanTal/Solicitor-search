import axios from "axios";
import type { ReportResponse } from "../Models/ReportResponse";

const api = axios.create({
  baseURL: "http://localhost:5006/api"
});

export async function getReport(
  locations: string[]
): Promise<ReportResponse> {

  const response = await api.post<ReportResponse>(
    "/report/getByLocations",
    locations);

  return response.data;
}