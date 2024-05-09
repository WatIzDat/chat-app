import { fetchJoinedDiscussionsByUser } from "@/app/lib/data";
import { auth } from "@clerk/nextjs/server";
import { redirect } from "next/navigation";

export default async function Page({ params }: { params: { id: string } }) {
    const joinedDiscussions: { id: string; name: string }[] =
        await fetchJoinedDiscussionsByUser(
            auth().sessionClaims?.userId as string
        );

    if (
        !joinedDiscussions ||
        !joinedDiscussions.map((d) => d.id).includes(params.id)
    ) {
        return redirect(`/join-discussion/${params.id}`);
    }

    return <div>{params.id}</div>;
}
