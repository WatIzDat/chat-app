import { fetchCreatedDiscussionsByUser } from "@/app/lib/data";

export async function GET(request: Request) {
    const result = await fetchCreatedDiscussionsByUser();

    return Response.json({ result });
}
