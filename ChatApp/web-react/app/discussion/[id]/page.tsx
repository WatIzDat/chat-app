import { fetchJoinedDiscussionsByUser } from "@/app/lib/data";
import { redirect } from "next/navigation";

export default async function Page({ params }: { params: { id: string } }) {
    const joinedDiscussions: { id: string; name: string }[] =
        await fetchJoinedDiscussionsByUser();

    if (
        !joinedDiscussions ||
        !joinedDiscussions.map((d) => d.id).includes(params.id)
    ) {
        return redirect(`/join-discussion/${params.id}`);
    }

    return <div>{params.id}</div>;
}
