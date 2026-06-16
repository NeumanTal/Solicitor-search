import type { SolicitorResponse } from "./SolicitorResponse";

export interface ReportResponse {
  solicitors: SolicitorResponse[];
  totalSolicitors: number;
  averageRating: number | null;
  topRatedSolicitor: SolicitorResponse | null;
}