export default function Page({ params }: { params: { id: string } }) {
    return <div>Join Discussion: {params.id}</div>;
}
